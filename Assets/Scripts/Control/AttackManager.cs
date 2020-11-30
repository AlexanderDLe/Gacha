using RPG.AI;
using RPG.Attributes;
using RPG.Characters;
using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class AttackManager : MonoBehaviour
    {
        Animator animator = null;
        BaseStats baseStats = null;
        AudioPlayer audioPlayer = null;
        ObjectPooler objectPooler = null;
        RaycastMousePosition raycaster = null;

        private void Start()
        {
            enemyLayer = LayerMask.GetMask("Enemy");
        }

        public void LinkReferences(AudioPlayer audioPlayer, RaycastMousePosition raycaster, Animator animator, ObjectPooler objectPooler)
        {
            this.audioPlayer = audioPlayer;
            this.raycaster = raycaster;
            this.animator = animator;
            this.objectPooler = objectPooler;
        }

        public void Initialize(CharacterManager character, BaseStats baseStats)
        {
            PlayableCharacter_SO script = character.script;

            this.baseStats = baseStats;
            this.weapon = character.weapon;
            this.fightingType = script.fightingType;
            this.hitboxPoint = weapon.hitboxPoint;

            this.numberOfAutoAttackHits = character.numberOfAutoAttackHits;
            this.autoAttackArray = character.autoAttackArray;
            this.autoAttackDamageFractions = script.autoAttackDamageFractions;
            InitializeAutoAttackFX(script);

            if (this.fightingType == FightingType.Projectile)
            {
                this.projectile_SO = script.projectile;
            }
            if (this.fightingType == FightingType.Melee)
            {
                this.autoAttackHitRadiuses = script.autoAttackHitRadiuses;
            }
        }

        private void InitializeAutoAttackFX(PlayableCharacter_SO script)
        {
            this.autoAttackVFX = script.autoAttackVFX;
            this.weakAttackAudio = script.weakAttackAudio;
            this.mediumAttackAudio = script.mediumAttackAudio;
        }

        GameObject[] autoAttackVFX = null;
        AudioClip[] weakAttackAudio = null;
        AudioClip[] mediumAttackAudio = null;
        FightingType fightingType;
        Weapon weapon = null;
        Projectile_SO projectile_SO = null;
        LayerMask enemyLayer;
        Transform hitboxPoint = null;

        // Auto Attack Mechanics
        public bool isInAutoAttackState = false;
        public bool canTriggerNextAutoAttack = true;
        public int comboNum = 0;
        public int numberOfAutoAttackHits;
        public string[] autoAttackArray = null;

        public float[] autoAttackHitRadiuses = null;
        public float[] autoAttackDamageFractions = null;

        public void SetIsInAutoAttackState(bool value) => isInAutoAttackState = value;
        public bool GetCanTriggerNextAutoAttack() => canTriggerNextAutoAttack;
        public void SetCanTriggerNextAutoAttack(bool value) => canTriggerNextAutoAttack = value;
        public string[] GetAutoAttackArray() => autoAttackArray;
        public void SetComboNum(int i) => comboNum = i;
        public int GetComboNum() => comboNum;

        public bool IsInAutoAttackAnimation()
        {
            for (int i = 0; i < numberOfAutoAttackHits; i++)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(autoAttackArray[i]))
                {
                    return true;
                }
            }
            return false;
        }

        // Animation Event Triggered Functions
        public void Attack1()
        {
            audioPlayer.SelectAndPlayCharacterClip(weakAttackAudio);
            Instantiate(autoAttackVFX[0], transform.position, transform.rotation);
            AutoAttack(0);
        }
        public void Attack2()
        {
            audioPlayer.SelectAndPlayCharacterClip(mediumAttackAudio);
            Instantiate(autoAttackVFX[1], transform.position, transform.rotation);
            AutoAttack(1);
        }
        public void AttackStart() { }
        public void AttackEnd()
        {
            SetCanTriggerNextAutoAttack(true);
            SetIsInAutoAttackState(false);
        }

        public void AutoAttack(int comboIndex)
        {
            if (fightingType == FightingType.Melee) InflictMeleeDamage(comboIndex);
            if (fightingType == FightingType.Projectile) ShootProjectile(comboIndex);
        }
        private void ShootProjectile(int comboIndex)
        {
            Projectile proj = objectPooler.SpawnFromPool(projectile_SO.prefab.name).GetComponent<Projectile>();

            LayerMask terrainLayer = LayerMask.GetMask("Terrain");
            RaycastHit ray = raycaster.GetRaycastMousePoint(terrainLayer);

            proj.Initialize(transform.position, ray.point, projectile_SO.speed, baseStats.GetDamage(), "Enemy", projectile_SO.maxLifeTime);
        }
        public void InflictMeleeDamage(int comboIndex)
        {
            print("Inflict melee called");
            float radius = autoAttackHitRadiuses[comboIndex];
            Collider[] hitEnemies = Physics.OverlapSphere(hitboxPoint.position, radius, enemyLayer);

            foreach (Collider enemy in hitEnemies)
            {
                print("Enemy Detected");
                AIManager AIEnemy = enemy.GetComponent<AIManager>();
                float damage = Mathf.Round(baseStats.GetDamage() * autoAttackDamageFractions[comboIndex]);

                AIEnemy.TakeDamage((int)damage);
            }
        }

    }
}