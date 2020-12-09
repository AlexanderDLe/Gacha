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
        AIManager AIManager = null;
        ObjectPooler objectPooler = null;
        GameObject player = null;
        BaseStats baseStats = null;
        GameObject prefab = null;
        MeleeAttacker meleeAttacker = null;
        public LayerMask playerLayer;
        public FightTypeEnum fightingType;
        public Weapon weapon;

        public float weaponRange = 1f;
        public float attackCooldownTime = 3f;
        public bool inAttackCooldown = false;
        public bool isInAttackAnimation = false;
        public float attackCooldownCounter = 0f;
        public float hitboxRadius = 1f;
        public Transform hitboxPoint = null;
        EnemyCharacter_SO script = null;

        [Header("Projectile")]
        public Transform projectileSpawnTransform;
        Projectile projectile = null;
        public float projectileSpeed = 1f;
        public float projectileLifetime = 5;

        public void Initialize(EnemyCharacter_SO script, ObjectPooler objectPooler, GameObject player, BaseStats baseStats, GameObject prefab)
        {
            this.script = script;
            this.AIManager = GetComponent<AIManager>();
            this.meleeAttacker = GetComponent<MeleeAttacker>();
            this.objectPooler = objectPooler;
            this.player = player;
            this.baseStats = baseStats;
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
            this.hitboxRadius = script.hitboxRadius;

            if (fightingType == FightTypeEnum.Projectile)
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
            if (fightingType == FightTypeEnum.Projectile) ShootProjectile();
            if (fightingType == FightTypeEnum.Melee) InflictMelee();
        }

        private void InflictMelee()
        {
            float damage = Mathf.Round(baseStats.GetDamage());
            meleeAttacker.Strike(hitboxPoint.position, hitboxRadius, playerLayer, damage);
        }

        private void ShootProjectile()
        {
            Projectile proj = objectPooler.SpawnFromPool(projectile.name).GetComponent<Projectile>();
            string layerToHarm = "Player";

            proj.Initialize(projectileSpawnTransform.position, player.transform.position, projectileSpeed, baseStats.GetDamage(), projectileLifetime, layerToHarm);
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