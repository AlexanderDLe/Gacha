using System;
using RPG.Attributes;
using RPG.Characters;
using RPG.Combat;
using RPG.Control;
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

        [Header("Projectile")]
        public Transform projectileSpawnTransform;
        Projectile projectile;
        public float projectileSpeed = 1f;
        public float projectileLifetime = 5;

        public void Initialize(EnemyCharacter_SO script, ObjectPooler objectPooler, GameObject player, Stats stats, GameObject prefab)
        {
            this.script = script;
            this.AIManager = GetComponent<AIManager>();
            this.objectPooler = objectPooler;
            this.player = player;
            this.stats = stats;
            this.prefab = prefab;

            this.playerLayer = LayerMask.GetMask("Player");
            this.attackCooldownTime = script.attackCooldownTime;
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

                this.projectile = projectilePrefab.GetComponent<Projectile>();

                this.projectileSpeed = script.projectile_SO.speed;
                this.projectileLifetime = script.projectile_SO.maxLifeTime;
                this.projectileSpawnTransform = hitboxPoint;
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

            IAOE_Effect deliverEffects = new IAOE_Execute(hitboxPoint.position, radius, playerLayer, effectPackage);
            deliverEffects.ApplyEffect();
        }

        private void ShootProjectile()
        {
            Projectile proj = objectPooler.SpawnFromPool(projectile.name).GetComponent<Projectile>();
            LayerMask layerToHarm = LayerMask.GetMask("Player");

            proj.Initialize(projectileSpawnTransform.position, player.transform.position, projectileSpeed, effectPackage, projectileLifetime, layerToHarm);
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