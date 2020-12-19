using System;
using RPG.Attributes;
using RPG.Characters;
using RPG.Combat;
using UnityEngine;

namespace RPG.AI
{
    public class AIAttackManager : MonoBehaviour
    {
        AIManager AIManager;
        ObjectPooler objectPooler;
        GameObject player;
        Stats stats;
        GameObject prefab;
        ProjectileLauncher projectileLauncher;
        public LayerMask playerLayer;
        public AttackTypeEnum fightingType;
        public Weapon weapon;

        public float weaponRange = 1f;
        public float attackCooldownTime = 3f;
        public bool inAttackCooldown = false;
        public bool isInAttackAnimation = false;
        public float attackCooldownCounter = 0f;
        public float radius = 1f;
        public Transform hitboxPoint;
        public EffectPackage effectPackage;
        EnemyCharacter_SO script;

        public void Initialize(EnemyCharacter_SO script, ObjectPooler objectPooler, GameObject player, Stats stats, GameObject prefab, ProjectileLauncher projectileLauncher)
        {
            this.script = script;
            this.objectPooler = objectPooler;
            this.player = player;
            this.stats = stats;
            this.prefab = prefab;
            this.projectileLauncher = projectileLauncher;

            this.playerLayer = LayerMask.GetMask("Player");
            this.weapon = script.weapon;
            this.weaponRange = script.weaponRange;

            InitializeFighter();
        }

        public void InitializeFighter()
        {
            fightingType = script.fightingType;

            this.hitboxPoint = prefab.GetComponent<WeaponHolder>().holdWeapon_GO.transform;
            this.radius = script.hitboxRadius;
            this.effectPackage = script.attackEffect;

            if (fightingType == AttackTypeEnum.Projectile)
            {
                GameObject projectilePrefab = script.projectile_SO.prefab;
                objectPooler.AddToPool(projectilePrefab, 10);
            }
        }

        public event Action OnAttackTriggered;
        public void TriggerAttack()
        {
            isInAttackAnimation = true;
            OnAttackTriggered();
        }

        public void Attack()
        {
            if (fightingType == AttackTypeEnum.Projectile) ShootProjectile();
            if (fightingType == AttackTypeEnum.Melee) InflictMelee();
        }

        private void InflictMelee()
        {
            float damage = Mathf.Round(stats.GetDamage());

            AOE_Effect deliverEffects = new AOE_Execute(stats, hitboxPoint.position, radius, playerLayer, effectPackage);
            deliverEffects.ApplyEffect();
        }

        private void ShootProjectile()
        {
            projectileLauncher.Launch(stats, objectPooler, script.projectile_SO, hitboxPoint.position, player.transform.position, playerLayer, effectPackage);
        }

        public void AttackStart() { }
        public void AttackActivate()
        {
            Attack();
        }
        public void AttackEnd()
        {
            isInAttackAnimation = false;
        }
    }
}