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
        AudioManager audioPlayer = null;
        ObjectPooler objectPooler = null;
        RaycastMousePosition raycaster = null;
        AOECreator aoeCreator = null;
        ProjectileLauncher projectileLauncher = null;

        private void Start()
        {
            enemyLayer = LayerMask.GetMask("Enemy");
        }

        public void LinkReferences(AudioManager audioPlayer, RaycastMousePosition raycaster, Animator animator, ObjectPooler objectPooler, AOECreator aoeCreator, ProjectileLauncher projectileLauncher)
        {
            this.audioPlayer = audioPlayer;
            this.raycaster = raycaster;
            this.animator = animator;
            this.objectPooler = objectPooler;
            this.aoeCreator = aoeCreator;
            this.projectileLauncher = projectileLauncher;
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

            if (this.fightingType == FightTypeEnum.Projectile)
            {
                this.projectile_SO = script.projectile;
            }
            if (this.fightingType == FightTypeEnum.Melee)
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
        FightTypeEnum fightingType;
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
            audioPlayer.PlayAudio(AudioEnum.Character, weakAttackAudio);
            Instantiate(autoAttackVFX[0], transform.position, transform.rotation);
            AutoAttack(0);
        }
        public void Attack2()
        {
            audioPlayer.PlayAudio(AudioEnum.Character, mediumAttackAudio);
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
            if (fightingType == FightTypeEnum.Melee) InflictMeleeDamage(comboIndex);
            if (fightingType == FightTypeEnum.Projectile) ShootProjectile(comboIndex);
        }
        private void ShootProjectile(int comboIndex)
        {
            LayerMask terrainLayer = LayerMask.GetMask("Terrain");
            RaycastHit ray = raycaster.GetRaycastMousePoint(terrainLayer);

            string prefabName = projectile_SO.prefab.name;
            Vector3 projOrigin = transform.position;
            Vector3 projDestination = ray.point;

            float speed = projectile_SO.speed;
            float damage = CalculateDamage(comboIndex);
            float lifetime = projectile_SO.maxLifeTime;
            LayerMask layerToHarm = LayerMask.GetMask("Enemy");

            projectileLauncher.Shoot(prefabName, projOrigin, projDestination, speed, damage, lifetime, layerToHarm);
        }

        public void InflictMeleeDamage(int comboIndex)
        {
            // Configure Melee Parameters then trigger Melee Strike
            float radius = autoAttackHitRadiuses[comboIndex];
            float damage = CalculateDamage(comboIndex);

            aoeCreator.Invoke(hitboxPoint.position, radius, enemyLayer, damage);
        }

        private float CalculateDamage(int comboIndex)
        {
            return Mathf.Round(baseStats.GetDamage() * autoAttackDamageFractions[comboIndex]);
        }
    }
}