using System;
using RPG.Characters;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.AIControl
{
    public class EnemyAIController : MonoBehaviour
    {
        GameObject player = null;
        StateMachine stateMachine = null;
        NavMeshAgent navMeshAgent = null;
        Animator animator = null;
        EnemyAIManager AIManager = null;
        Vector3 guardPosition;

        private void Awake()
        {
            this.stateMachine = GetComponent<StateMachine>();
            this.navMeshAgent = GetComponent<NavMeshAgent>();
            this.AIManager = GetComponent<EnemyAIManager>();
            this.animator = GetComponent<Animator>();
            this.player = GameObject.FindWithTag("Player");
        }

        private void Start()
        {
            guardPosition = transform.position;
            AIEnterIdleState();
        }

        private void OnEnable()
        {
            AIManager.OnDamageTaken += EnterFlinchState;
        }
        private void OnDisable()
        {
            AIManager.OnDamageTaken -= EnterFlinchState;
        }

        private void Update()
        {
            AIBehaviour();
            stateMachine.ExecuteStateUpdate();
            UpdateAnimator();
        }

        private void AIBehaviour()
        {
            if (AIManager.IsAggravated())
            {
                AggressiveBehaviour();
            }
            else if (AIManager.IsSuspicious())
            {
                SuspiciousState();
            }
            else
            {
                PassiveBehaviour();
            }
        }

        private void PassiveBehaviour()
        {
            if (!AIManager.CanMove()) return;
            AIManager.EndAggression();
            AIEnterMoveState(guardPosition, AIManager.movementSpeed);
        }

        private void AggressiveBehaviour()
        {
            if (!AIManager.isAggressive) AIManager.StartAggression();
            // SetAICombatStance(true);

            AIManager.timeSinceAggravated = 0;
            if (!AIManager.WithinAttackRange())
            {
                if (!AIManager.CanMove()) return;
                AIEnterChaseState(AIManager.movementSpeed);
            }
            else
            {
                if (!AIManager.CanAttack())
                {
                    if (AIManager.CanEnterCombatStance()) AIEnterCombatStance();
                    return;
                }
                AIEnterAttackState();
            }
        }

        public void EnterFlinchState()
        {
            stateMachine.changeState(new AI_Flinch(AIManager, animator), StateEnum.Flinch);
        }
        public void AIEnterIdleState()
        {
            stateMachine.changeState(new AI_Idler(), StateEnum.Idle);
        }
        public void AIEnterCombatStance()
        {
            stateMachine.changeState(new AI_CombatStance(navMeshAgent, animator), StateEnum.CombatStance);
        }
        public void SuspiciousState()
        {
            stateMachine.changeState(new AI_Idler(), StateEnum.Idle);
        }
        public void AIEnterChaseState(float movementSpeed)
        {
            stateMachine.changeState(new AI_Mover(player, navMeshAgent, movementSpeed), StateEnum.Chase);
        }
        public void AIEnterMoveState(Vector3 destination, float movementSpeed)
        {
            stateMachine.changeState(new AI_Mover(destination, navMeshAgent, movementSpeed), StateEnum.Move);
        }
        public void AIEnterAttackState()
        {
            stateMachine.changeState(new AI_Attacker(player, AIManager, animator), StateEnum.Attack);
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }
    }
}