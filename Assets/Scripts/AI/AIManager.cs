using System;
using System.Collections;
using RPG.Attributes;
using RPG.Characters;
using RPG.Combat;
using RPG.Control;
using RPG.Core;
using Sirenix.OdinInspector;
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
        BaseStats baseStats = null;
        DamageTextSpawner damageTextSpawner = null;
        public EnemyCharacter_SO enemy_SO = null;
        public AIAggroManager aggro = null;
        public AIAttackManager attacker = null;
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
            objectPooler = GameObject.FindWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        }
        private void Start()
        {
            BuildCharacter(enemy_SO);
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
            InitializeModel(prefab, animator, enemy_SO);

            baseStats.Initialize(enemy_SO);

            aggro.Initialize(player, enemy_SO);
            attacker.Initialize(enemy_SO, objectPooler, player, baseStats, prefab);
        }

        public void InitializeModel(GameObject prefab, Animator animator, EnemyCharacter_SO script)
        {
            prefab = Instantiate(script.prefab, transform);

            animator.avatar = script.characterAvatar;

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
            if (isFlinching) return false;
            if (attacker.isInAttackAnimation) return false;
            return true;
        }
        public bool CanAttack()
        {
            if (isFlinching) return false;
            if (attacker.inAttackCooldown) return false;
            if (attacker.isInAttackAnimation) return false;
            return true;
        }
        public bool CanEnterCombatStance()
        {
            if (isFlinching) return false;
            if (attacker.isInAttackAnimation) return false;
            return true;
        }
        #endregion

        #region Attributes
        public GameObject prefab = null;
        public float movementSpeed = 4f;
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
                if (isFlinching) attacker.attackCooldownCounter = attacker.attackCooldownTime;
                attacker.attackCooldownCounter -= Time.deltaTime;
                yield return null;
            }
            attacker.inAttackCooldown = false;
        }
        #endregion
    }
}