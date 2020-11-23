using RPG.Core;
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
        EnemyCharacter_SO enemy_SO = null;

        private void Awake()
        {
            stateMachine = GetComponent<StateMachine>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            player = GameObject.FindWithTag("Player");
        }
        private void Start()
        {
            BuildCharacter();
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
        public void BuildCharacter()
        {
            this.movementSpeed = enemy_SO.movementSpeed;
            this.attackCooldown = enemy_SO.attackCooldown;
            this.weaponRange = enemy_SO.weaponRange;
            this.hasProjectile = enemy_SO.projectile_SO != null;
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
            if (IsInAttackAnimation()) return false;
            return true;
        }
        #endregion

        #region Attributes
        public float movementSpeed = 4f;
        public float attackCooldown = 1f;
        public float weaponRange = 1f;
        public bool hasProjectile = false;
        #endregion

        #region Enemy Aggro
        public float chaseDistance = 5f;
        public float distanceToPlayer = 0f;
        public float timeSinceLastSawPlayer = Mathf.Infinity;
        public float timeSinceAggravated = Mathf.Infinity;
        public float aggroCooldownTime = 3f;
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

        #region Attack

        #endregion

        #region Animator
        public bool IsInAttackAnimation()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("attack");
        }
        public void AttackStart() { }
        public void AttackActivate() { }
        public void AttackEnd() { }
        #endregion
    }
}