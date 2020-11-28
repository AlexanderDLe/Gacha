using System;
using System.Collections;
using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
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
        DamageTextSpawner damageTextSpawner = null;
        [SerializeField] EnemyCharacter_SO enemy_SO = null;
        public GameObject placeholder = null;

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            stateMachine = GetComponent<StateMachine>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            damageTextSpawner = GetComponent<DamageTextSpawner>();
            player = GameObject.FindWithTag("Player");
            objectPooler = GameObject.FindWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        }
        private void Start()
        {
            BuildCharacter(enemy_SO);
            playerLayer = LayerMask.GetMask("Player");
            placeholder.SetActive(false);
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
            InitializeModel(enemyScriptObj);

            baseStats.Initialize(enemyScriptObj);
            this.attackCooldownTime = enemyScriptObj.attackCooldownTime;
            this.attackCooldownTime = enemyScriptObj.attackCooldownTime;
            this.weapon = enemyScriptObj.weapon;
            this.weaponRange = enemyScriptObj.weaponRange;
            this.chaseDistance = enemyScriptObj.chaseDistance;
            this.fightingType = enemyScriptObj.fightingType;

            InitializeFightType(enemyScriptObj);
        }

        private void InitializeModel(EnemyCharacter_SO enemyScriptObj)
        {
            this.prefab = Instantiate(enemyScriptObj.prefab, transform);

            animator.avatar = enemyScriptObj.characterAvatar;
            if (enemyScriptObj.animatorOverride != null)
            {
                animator.runtimeAnimatorController = enemyScriptObj.animatorOverride;
            }
            animator.Rebind();
        }

        private void InitializeFightType(EnemyCharacter_SO enemyScriptObj)
        {
            if (fightingType == FightingType.Projectile)
            {
                InitializeProjectile(enemyScriptObj);
            }
            if (fightingType == FightingType.Melee)
            {
                InitializeMelee(enemyScriptObj);
            }
        }

        private void InitializeMelee(EnemyCharacter_SO enemyScriptObj)
        {
            this.hitboxRadius = enemyScriptObj.hitboxRadius;
            this.hitboxPoint = prefab.GetComponent<WeaponHolder>().transform;
        }

        private void InitializeProjectile(EnemyCharacter_SO enemyScriptObj)
        {
            GameObject projectilePrefab = enemyScriptObj.projectile_SO.prefab;

            objectPooler.AddToPool(projectilePrefab, 10);

            projectile = projectilePrefab.GetComponent<Projectile>();

            projectileSpeed = enemyScriptObj.projectile_SO.speed;
            projectileLifetime = enemyScriptObj.projectile_SO.maxLifeTime;
        }
        #endregion

        #region Permissions
        public bool CanMove()
        {
            if (isFlinching) return false;
            if (isInAttackAnimation) return false;
            return true;
        }
        public bool CanAttack()
        {
            if (isFlinching) return false;
            if (inAttackCooldown) return false;
            if (isInAttackAnimation) return false;
            return true;
        }
        public bool CanEnterCombatStance()
        {
            if (isFlinching) return false;
            if (isInAttackAnimation) return false;
            return true;
        }
        #endregion

        #region Attributes
        [FoldoutGroup("Attributes")]
        public GameObject prefab = null;
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
        [FoldoutGroup("Aggro")]
        public bool isAggressive = false;

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
        public void StartAggression()
        {
            isAggressive = true;
            StartCoroutine(StartAttackCooldown());
        }
        public void EndAggression()
        {
            isAggressive = false;
        }
        #endregion

        #region Take Damage
        public bool isFlinching = false;
        public event Action OnDamageTaken;
        public void TakeDamage(int damage)
        {
            baseStats.TakeDamage(damage);
            damageTextSpawner.SpawnText(damage);
            OnDamageTaken();
        }
        public void SetIsFlinching(bool value)
        {
            isFlinching = value;
        }
        public void FlinchStart() { }
        public void FlinchEnd()
        {
            SetIsFlinching(false);
        }
        #endregion

        #region Attack
        [FoldoutGroup("Attack")]
        public LayerMask playerLayer;
        [FoldoutGroup("Attack")]
        public FightingType fightingType;
        [FoldoutGroup("Attack")]
        public Weapon weapon;
        [FoldoutGroup("Attack")]
        public float weaponRange = 1f;
        [FoldoutGroup("Attack")]
        public bool inAttackCooldown = false;
        [FoldoutGroup("Attack")]
        public float attackCooldownTime = 3f;
        [FoldoutGroup("Attack")]
        public bool isInAttackAnimation = false;
        [FoldoutGroup("Attack")]
        public float attackCooldownCounter = 0f;
        [FoldoutGroup("Attack")]
        public float hitboxRadius = 1f;
        [FoldoutGroup("Attack")]
        public Transform hitboxPoint = null;

        [Header("Projectile")]
        [FoldoutGroup("Attack")]
        public Transform projectileSpawnTransform;
        [FoldoutGroup("Attack")]
        Projectile projectile = null;
        [FoldoutGroup("Attack")]
        public float projectileSpeed = 1f;
        [FoldoutGroup("Attack")]
        public float projectileLifetime = 5;


        public void TriggerAttack()
        {
            isInAttackAnimation = true;
            StartCoroutine(StartAttackCooldown());
        }
        IEnumerator StartAttackCooldown()
        {
            attackCooldownCounter = attackCooldownTime;
            inAttackCooldown = true;
            while (attackCooldownCounter > 0)
            {
                if (isFlinching) attackCooldownCounter = attackCooldownTime;
                attackCooldownCounter -= Time.deltaTime;
                yield return null;
            }
            inAttackCooldown = false;
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
            if (!objectPooler) Debug.Log("Object Pooler not found.");

            // Spawn a GameObject from ObjectPool then access as Projectile
            Projectile proj = objectPooler.SpawnFromPool(projectile.name).GetComponent<Projectile>();

            if (!proj) Debug.Log("Projectile not found.");

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
        #endregion
    }
}