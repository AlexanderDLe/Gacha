using System;
using System.Collections;
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

        public LayerMask playerLayer;
        public FightingType fightingType;
        public Weapon weapon;
        public float weaponRange = 1f;
        public bool inAttackCooldown = false;
        public float attackCooldownTime = 3f;
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

            GameObject projectilePrefab = script.projectile_SO.prefab;

            if (fightingType == FightingType.Projectile)
            {
                objectPooler.AddToPool(projectilePrefab, 10);
            }

            projectile = projectilePrefab.GetComponent<Projectile>();

            projectileSpeed = script.projectile_SO.speed;
            projectileLifetime = script.projectile_SO.maxLifeTime;

            hitboxPoint = prefab.GetComponent<WeaponHolder>().holdWeapon_GO.transform;
            hitboxRadius = script.hitboxRadius;

            projectileSpawnTransform = hitboxPoint;
        }

        public event Action OnAttackTriggered;
        public void TriggerAttack()
        {
            isInAttackAnimation = true;
            OnAttackTriggered();
        }

        public void Attack()
        {
            if (fightingType == FightingType.Projectile) ShootProjectile();
            if (fightingType == FightingType.Melee) InflictMelee();
        }

        private void InflictMelee()
        {
            Collider[] hitResults = Physics.OverlapSphere(hitboxPoint.position, hitboxRadius, playerLayer);

            foreach (Collider hit in hitResults)
            {
                print(hit);
                BaseStats player = hit.GetComponent<StateManager>().currBaseStats;
                float damage = Mathf.Round(baseStats.GetDamage());

                player.TakeDamage((int)damage);
            }
        }

        private void ShootProjectile()
        {
            Projectile proj = objectPooler.SpawnFromPool(projectile.name).GetComponent<Projectile>();

            proj.Initialize(projectileSpawnTransform.position, player.transform.position, projectileSpeed, baseStats.GetDamage(), "Player", projectileLifetime);
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