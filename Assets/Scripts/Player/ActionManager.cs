using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using RPG.Characters;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.AI;

namespace RPG.Control
{
    public class ActionManager : MonoBehaviour
    {
        public GameObject environment = null;
        ObjectPooler objectPooler = null;
        RaycastMousePosition raycaster = null;
        AudioSource audioSource = null;
        AudioSource characterAudioSource = null;
        AudioSource actionAudioSource = null;
        BaseStats baseStats = null;

        #region Initializations
        private void Awake()
        {
            raycaster = GetComponent<RaycastMousePosition>();
            audioSource = GetComponent<AudioSource>();
            characterAudioSource = gameObject.AddComponent<AudioSource>();
            actionAudioSource = gameObject.AddComponent<AudioSource>();
            objectPooler = GameObject.FindWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        }

        private void Start()
        {
            this.enemyLayer = LayerMask.GetMask("Enemy");
        }

        public void Initialize(CharacterManager character, BaseStats baseStats)
        {
            PlayableCharacter_SO char_SO = character.script;
            Weapon weapon = character.weapon;

            InitializeAttackAttributes(char_SO, baseStats, weapon);
            InitializeFX(char_SO);
        }

        private void InitializeAttackAttributes(PlayableCharacter_SO char_SO, BaseStats baseStats, Weapon weapon)
        {
            this.baseStats = baseStats;
            this.weapon = weapon;
            this.hitboxPoint = weapon.hitboxPoint;
            this.fightingType = char_SO.fightingType;
            this.autoAttackDamageFraction = char_SO.autoAttackDamageFractions;

            if (this.fightingType == FightingType.Projectile)
            {
                this.projectile_SO = char_SO.projectile;
            }
            if (this.fightingType == FightingType.Melee)
            {
                this.autoAttackHitRadiuses = char_SO.autoAttackHitRadiuses;
            }
        }

        private void InitializeFX(PlayableCharacter_SO char_SO)
        {
            this.dashAudio = char_SO.dashAudio;

            this.autoAttackVFX = char_SO.autoAttackVFX;
            this.weakAttackAudio = char_SO.weakAttackAudio;
            this.mediumAttackAudio = char_SO.mediumAttackAudio;

            this.movementSkillVFX = char_SO.movementSkill.skillVFX;
            this.movementSkillVocalAudio = char_SO.movementSkill.skillVocalAudio;
            this.movementSkillActionAudio = char_SO.movementSkill.skillActionAudio;

            this.primarySkillVFX = char_SO.primarySkill.skillVFX;
            this.primarySkillVocalAudio = char_SO.primarySkill.skillVocalAudio;
            this.primarySkillActionAudio = char_SO.primarySkill.skillActionAudio;

            this.ultimateSkillVFX = char_SO.ultimateSkill.skillVFX;
            this.ultimateSkillVocalAudio = char_SO.ultimateSkill.skillVocalAudio;
            this.ultimateSkillActionAudio = char_SO.ultimateSkill.skillActionAudio;
        }
        #endregion

        #region Utility
        private bool RandomlyDecideIfPlay()
        {
            return Random.Range(0f, 10f) > 5f;
        }
        private void SelectAndPlayCharacterClip(AudioClip[] audioClips)
        {
            int clipNumber = Random.Range(0, audioClips.Length);
            characterAudioSource.PlayOneShot(audioClips[clipNumber]);
        }
        private void SelectAndPlayActionClip(AudioClip[] audioClips)
        {
            int clipNumber = Random.Range(0, audioClips.Length);
            actionAudioSource.PlayOneShot(audioClips[clipNumber]);
        }
        #endregion

        #region Swap Character
        [FoldoutGroup("Character Swap")]
        [SerializeField] GameObject swapVisualFX = null;
        [FoldoutGroup("Character Swap")]
        [SerializeField] AudioClip swapAudioFX = null;

        public void ActivateSwapFX()
        {
            GameObject swapVFX = Instantiate(swapVisualFX, transform);
            swapVFX.transform.SetParent(environment.transform);
            actionAudioSource.PlayOneShot(swapAudioFX);
        }
        #endregion

        #region Dash
        private AudioClip[] dashAudio = null;
        private bool dashAudioJustPlayed = false;

        private void DashStart()
        {
            if (!RandomlyDecideIfPlay()) return;
            if (characterAudioSource.isPlaying || dashAudioJustPlayed) return;
            SelectAndPlayCharacterClip(dashAudio);
            StartCoroutine(TriggerDashAudio());
        }
        IEnumerator TriggerDashAudio()
        {
            dashAudioJustPlayed = true;
            yield return new WaitForSeconds(5);
            dashAudioJustPlayed = false;
        }
        #endregion

        #region Auto Attack
        public FightingType fightingType;
        public Weapon weapon = null;
        public LayerMask enemyLayer;
        GameObject[] autoAttackVFX = null;
        AudioClip[] weakAttackAudio = null;
        AudioClip[] mediumAttackAudio = null;

        public Projectile_SO projectile_SO = null;
        public Transform hitboxPoint = null;
        public float[] autoAttackHitRadiuses = null;
        public float[] autoAttackDamageFraction = null;

        public GameObject hitboxDebugSphere = null;
        public void SpawnHitboxRadiusDebug()
        {
            GameObject debug_GO = Instantiate(hitboxDebugSphere, hitboxPoint);
            debug_GO.transform.SetParent(environment.transform);

            Vector3 hitboxScaling = new Vector3(weapon.hitboxDebugRadius * 2, weapon.hitboxDebugRadius * 2, weapon.hitboxDebugRadius * 2);
            debug_GO.transform.localScale = hitboxScaling;
        }

        public void AutoAttack(int index)
        {
            if (fightingType == FightingType.Melee) InflictMeleeDamage(index);
            if (fightingType == FightingType.Projectile) ShootProjectile(index);
        }

        private void ShootProjectile(int index)
        {
            if (!objectPooler) Debug.Log("Object Pooler not found.");
            // Spawn a GameObject from ObjectPool then access as Projectile
            Projectile proj = objectPooler.SpawnFromPool(projectile_SO.prefab.name).GetComponent<Projectile>();

            if (!proj) Debug.Log("Projectile not found.");
            LayerMask terrainLayer = LayerMask.GetMask("Terrain");
            RaycastHit ray = raycaster.GetRaycastMousePoint(terrainLayer);

            proj.Initialize(transform.position, ray.point, projectile_SO.speed, baseStats.GetDamage(), "Enemy", projectile_SO.maxLifeTime);
        }

        public void InflictMeleeDamage(int index)
        {
            float radius = autoAttackHitRadiuses[index];
            Collider[] hitEnemies = Physics.OverlapSphere(hitboxPoint.position, radius, enemyLayer);

            foreach (Collider enemy in hitEnemies)
            {
                AIManager AIEnemy = enemy.GetComponent<AIManager>();
                float damage = Mathf.Round(baseStats.GetDamage() * autoAttackDamageFraction[index]);

                AIEnemy.TakeDamage((int)damage);
            }
        }

        public void Attack1()
        {
            SelectAndPlayCharacterClip(weakAttackAudio);
            Instantiate(autoAttackVFX[0], transform.position, transform.rotation);
            AutoAttack(0);
        }
        public void Attack2()
        {
            SelectAndPlayCharacterClip(mediumAttackAudio);
            Instantiate(autoAttackVFX[1], transform.position, transform.rotation);
            AutoAttack(1);
        }
        #endregion

        #region Movement Skill
        AudioClip movementSkillVocalAudio;
        AudioClip movementSkillActionAudio;
        GameObject movementSkillVFX = null;
        public void MovementSkillStart()
        {
            Instantiate(movementSkillVFX, transform.position, transform.rotation);
            actionAudioSource.PlayOneShot(movementSkillActionAudio);
            if (RandomlyDecideIfPlay())
            {
                characterAudioSource.PlayOneShot(movementSkillVocalAudio);
            }
        }
        public void MovementSkillEnd() { }
        #endregion

        #region Primary Skill
        [FoldoutGroup("Primary Skill FX")]
        GameObject primarySkillVFX = null;
        [FoldoutGroup("Primary Skill FX")]
        AudioClip primarySkillActionAudio = null;
        [FoldoutGroup("Primary Skill FX")]
        AudioClip primarySkillVocalAudio = null;
        public void PrimarySkillStart()
        {
            characterAudioSource.PlayOneShot(primarySkillVocalAudio);
        }
        public void PrimarySkillActivate()
        {
            Instantiate(primarySkillVFX, transform.position, transform.rotation);
            actionAudioSource.PlayOneShot(primarySkillActionAudio);
        }
        #endregion

        #region Ultimate Skill
        GameObject ultimateSkillVFX = null;
        AudioClip ultimateSkillActionAudio = null;
        AudioClip ultimateSkillVocalAudio = null;
        public void UltimateSkillStart()
        {
            characterAudioSource.PlayOneShot(ultimateSkillVocalAudio);
        }
        public void UltimateSkillActivate()
        {
            Instantiate(ultimateSkillVFX, transform.position, transform.rotation);
            actionAudioSource.PlayOneShot(ultimateSkillActionAudio);
        }
        #endregion

        #region Footsteps
        [Header("Footsteps")]
        [SerializeField] AudioClip[] footstepClips = default;

        private void PlayRandomFootstepClip()
        {
            int num = Random.Range(0, footstepClips.Length);
            audioSource.PlayOneShot(footstepClips[num]);
        }
        private void Footstep()
        {
            PlayRandomFootstepClip();
        }
        #endregion
    }
}