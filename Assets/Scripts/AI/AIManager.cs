using System;
using System.Collections;
using RPG.Attributes;
using RPG.Characters;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.AI
{
    public class AIManager : MonoBehaviour
    {
        GameObject player = null;
        StateMachine stateMachine = null;
        NavMeshAgent navMeshAgent = null;
        Animator animator = null;
        ObjectPooler objectPooler = null;
        DamageTextSpawner damageTextSpawner = null;
        public BaseStats baseStats = null;
        public EnemyCharacter_SO script = null;
        public AIAggroManager aggro = null;
        public AIAttackManager attacker = null;
        public AIFlinchManager flincher = null;
        public GameObject placeholder = null;

        private void Awake()
        {
            stateMachine = GetComponent<StateMachine>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            damageTextSpawner = GetComponent<DamageTextSpawner>();
            baseStats = GetComponent<BaseStats>();
            player = GameObject.FindWithTag("Player");
            aggro = GetComponent<AIAggroManager>();
            attacker = GetComponent<AIAttackManager>();
            flincher = GetComponent<AIFlinchManager>();
            objectPooler = GameObject.FindWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        }
        private void Start()
        {
            BuildCharacter(script);
            placeholder.SetActive(false);
        }

        private void OnEnable()
        {
            aggro.OnStartAggression += StartAttackCooldownCoroutine;
            attacker.OnAttackTriggered += StartAttackCooldownCoroutine;
        }
        private void OnDisable()
        {
            aggro.OnStartAggression -= StartAttackCooldownCoroutine;
            attacker.OnAttackTriggered -= StartAttackCooldownCoroutine;
        }

        #region Initialization
        public void BuildCharacter(EnemyCharacter_SO enemy_SO)
        {
            InitializeModel(prefab, animator);

            baseStats.Initialize(enemy_SO);

            aggro.Initialize(player, enemy_SO);

            attacker.Initialize(enemy_SO, objectPooler, player, baseStats, prefab);
        }

        public void InitializeModel(GameObject prefab, Animator animator)
        {
            this.prefab = Instantiate(script.prefab, transform);

            this.animator.avatar = script.characterAvatar;

            if (script.animatorOverride != null)
            {
                animator.runtimeAnimatorController = script.animatorOverride;
            }

            animator.Rebind();
        }
        #endregion

        #region Permissions
        public bool CanMove()
        {
            if (flincher.isFlinching) return false;
            if (attacker.isInAttackAnimation) return false;
            return true;
        }
        public bool CanAttack()
        {
            if (flincher.isFlinching) return false;
            if (attacker.inAttackCooldown) return false;
            if (attacker.isInAttackAnimation) return false;
            return true;
        }
        public bool CanEnterCombatStance()
        {
            if (flincher.isFlinching) return false;
            if (attacker.isInAttackAnimation) return false;
            return true;
        }
        #endregion

        public GameObject prefab = null;

        public event Action OnDamageTaken;
        public void TakeDamage(int damage)
        {
            baseStats.TakeDamage(damage);
            damageTextSpawner.SpawnText(damage);
            OnDamageTaken();
        }

        public void StartAttackCooldownCoroutine()
        {
            StartCoroutine(StartAttackCooldown());
        }
        IEnumerator StartAttackCooldown()
        {
            attacker.attackCooldownCounter = attacker.attackCooldownTime;
            attacker.inAttackCooldown = true;
            while (attacker.attackCooldownCounter > 0)
            {
                if (flincher.isFlinching) attacker.attackCooldownCounter = attacker.attackCooldownTime;
                attacker.attackCooldownCounter -= Time.deltaTime;
                yield return null;
            }
            attacker.inAttackCooldown = false;
        }
    }
}