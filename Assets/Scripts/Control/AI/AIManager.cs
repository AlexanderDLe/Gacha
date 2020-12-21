using System;
using System.Collections;
using RPG.Attributes;
using RPG.Characters;
using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.AI
{
    public class AIManager : BaseManager
    {
        GameObject player = null;
        DamageTextSpawner damageTextSpawner = null;
        public EnemyCharacter_SO script = null;
        public AIAggroManager aggro = null;
        public AIAttackManager attacker = null;
        public AIFlinchManager flincher = null;
        public GameObject placeholder = null;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            damageTextSpawner = GetComponent<DamageTextSpawner>();
            stats = GetComponent<Stats>();
            aggro = GetComponent<AIAggroManager>();
            attacker = GetComponent<AIAttackManager>();
            flincher = GetComponent<AIFlinchManager>();
            effectExecuter = GetComponent<EffectExecutor>();
            player = GameObject.FindWithTag("Player");
            projectileLauncher = GetComponent<ProjectileLauncher>();
            debugPooler = GameObject.FindWithTag("DebugPooler").GetComponent<ObjectPooler>();
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

            stats.Initialize(enemy_SO);

            aggro.Initialize(player, enemy_SO);

            attacker.Initialize(aggro, enemy_SO, debugPooler, stats, prefab, projectileLauncher);
        }

        public void InitializeModel(GameObject prefab, Animator animator)
        {
            this.prefab = Instantiate(script.prefab, transform);

            this.animator.avatar = script.characterAvatar;

            if (script.animatorController != null)
            {
                animator.runtimeAnimatorController = script.animatorController;
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

        #region Functions
        public event Action OnDamageTaken;
        public override void TakeDamage(int damage)
        {
            stats.TakeDamage(damage);
            damageTextSpawner.SpawnText(damage);
            OnDamageTaken();
        }
        #endregion

        public void StartAttackCooldownCoroutine()
        {
            StartCoroutine(StartAttackCooldown());
        }
        IEnumerator StartAttackCooldown()
        {
            ResetAttackCountdown();
            attacker.inAttackCooldown = true;
            while (attacker.attackCooldownCounter > 0)
            {
                if (flincher.isFlinching) ResetAttackCountdown();
                attacker.attackCooldownCounter -= Time.deltaTime;
                yield return null;
            }
            attacker.inAttackCooldown = false;
        }

        private void ResetAttackCountdown()
        {
            attacker.attackCooldownCounter = attacker.attackCooldownTime;
        }
    }
}