using System;
using System.Collections;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Characters
{
    public class EnemyAIManager : MonoBehaviour
    {
        GameObject player = null;
        StateMachine stateMachine = null;
        NavMeshAgent navMeshAgent = null;
        Animator animator = null;
        ObjectPooler objectPooler = null;
        BaseStats baseStats = null;
        [SerializeField] EnemyCharacter_SO enemy_SO = null;

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            stateMachine = GetComponent<StateMachine>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            player = GameObject.FindWithTag("Player");
        }
        private void Start()
        {
            BuildCharacter(enemy_SO);
        }
        private void Update()
        {
            UpdateTimers();
        }
        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceAggravated += Time.deltaTime;
        }

        #region Initialization
        public void BuildCharacter(EnemyCharacter_SO enemyScriptObj)
        {
            baseStats.Initialize(enemyScriptObj);
            this.health = enemyScriptObj.health;
            this.attackCooldownTime = enemyScriptObj.attackCooldownTime;
            this.attackCooldownTime = enemyScriptObj.attackCooldownTime;
            this.weaponRange = enemyScriptObj.weaponRange;
            this.hasProjectile = enemyScriptObj.projectile_SO != null;
            this.damage = enemyScriptObj.baseDamage;
            this.chaseDistance = enemyScriptObj.chaseDistance;

            if (this.hasProjectile)
            {
                objectPooler = GameObject.FindWithTag("ObjectPooler").GetComponent<ObjectPooler>();

                projectile = enemyScriptObj.projectile_SO.prefab;
                projectileSpeed = enemyScriptObj.projectile_SO.speed;
                projectileLifetime = enemyScriptObj.projectile_SO.maxLifeTime;
            }
        }
        #endregion

        #region Permissions
        public bool CanMove()
        {
            if (IsInAttackAnimation()) return false;
            return true;
        }
        public bool CanAttack()
        {
            if (inAttackCooldown) return false;
            if (IsInAttackAnimation()) return false;
            return true;
        }
        #endregion

        #region Attributes
        [FoldoutGroup("Attributes")]
        public float health = 4f;
        [FoldoutGroup("Attributes")]
        public float movementSpeed = 4f;
        [FoldoutGroup("Attributes")]
        public float damage = 5f;
        #endregion

        #region Enemy Aggro
        [FoldoutGroup("Aggro")]
        public float chaseDistance = 5f;
        [FoldoutGroup("Aggro")]
        public float distanceToPlayer = 0f;
        [FoldoutGroup("Aggro")]
        public float timeSinceLastSawPlayer = Mathf.Infinity;
        [FoldoutGroup("Aggro")]
        public float timeSinceAggravated = Mathf.Infinity;
        [FoldoutGroup("Aggro")]
        public float aggroCooldownTime = 3f;
        [FoldoutGroup("Aggro")]
        public float suspicionTime = 3f;

        public void Aggravate()
        {
            timeSinceAggravated = 0;
        }
        public bool IsAggravated()
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            bool aggravated = timeSinceLastSawPlayer <= aggroCooldownTime;

            if (distanceToPlayer <= chaseDistance) timeSinceLastSawPlayer = 0;

            return distanceToPlayer <= chaseDistance || aggravated;
        }
        public bool IsSuspicious()
        {
            return timeSinceAggravated < suspicionTime;
        }

        public bool WithinAttackRange()
        {
            return distanceToPlayer <= weaponRange;
        }
        #endregion

        #region 
        [FoldoutGroup("Attack")]
        public float weaponRange = 1f;
        [FoldoutGroup("Attack")]
        public bool inAttackCooldown = false;
        [FoldoutGroup("Attack")]
        public float attackCooldownTime = 3f;

        [Header("Projectile")]
        [FoldoutGroup("Attack")]
        public Transform projectileSpawnTransform;
        [FoldoutGroup("Attack")]
        Projectile projectile = null;
        [FoldoutGroup("Attack")]
        public bool hasProjectile = false;
        [FoldoutGroup("Attack")]
        public float projectileSpeed = 1f;
        [FoldoutGroup("Attack")]
        public float projectileLifetime = 5;

        public void TriggerAttack()
        {
            StartCoroutine(StartAttackCooldown());
        }
        IEnumerator StartAttackCooldown()
        {
            inAttackCooldown = true;
            yield return new WaitForSeconds(attackCooldownTime);
            inAttackCooldown = false;
        }
        #endregion

        #region Animator
        public bool IsInAttackAnimation()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("attack");
        }
        public void AttackStart() { }
        public void AttackActivate()
        {
            if (hasProjectile)
            {
                if (!objectPooler) Debug.Log("Object Pooler not found.");
                // Spawn a GameObject from ObjectPool then access as Projectile
                Projectile proj = objectPooler.SpawnFromPool(projectile.name).GetComponent<Projectile>();

                if (!proj) Debug.Log("Projectile not found.");

                proj.Initialize(projectileSpawnTransform.position, player.transform.position, projectileSpeed, damage, "Player", projectileLifetime);

            }
        }
        public void AttackEnd() { }
        #endregion
    }
}