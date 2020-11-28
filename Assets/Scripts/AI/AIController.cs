using System;
using RPG.Characters;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.AI
{
    public class AIController : MonoBehaviour
    {
        GameObject player = null;
        StateMachine stateMachine = null;
        NavMeshAgent navMeshAgent = null;
        Animator animator = null;
        AIManager AIManager = null;
        Vector3 guardPosition;
        AIAggroManager aggro = null;

        private void Awake()
        {
            this.stateMachine = GetComponent<StateMachine>();
            this.navMeshAgent = GetComponent<NavMeshAgent>();
            this.AIManager = GetComponent<AIManager>();
            this.animator = GetComponent<Animator>();
            this.player = GameObject.FindWithTag("Player");
        }

        private void Start()
        {
            this.aggro = AIManager.aggro;
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
            if (aggro.IsAggravated())
            {
                AggressiveBehaviour();
            }
            else if (aggro.IsSuspicious())
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
            aggro.EndAggression();
            AIEnterMoveState(guardPosition, AIManager.movementSpeed);
        }

        private void AggressiveBehaviour()
        {
            if (!aggro.isAggressive) aggro.StartAggression();

            aggro.timeSinceAggravated = 0;
            if (!aggro.WithinAttackRange())
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
        public void SuspiciousState()
        {
            AIEnterCombatStance();
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