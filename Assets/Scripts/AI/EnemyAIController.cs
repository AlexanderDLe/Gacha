using System;
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

        private void Update()
        {
            if (AIManager.IsAggravated())
            {
                AggroBehaviour();
            }
            else if (AIManager.IsSuspicious())
            {
                print("Entering Sus State");
                AIEnterSuspiciousState();
            }
            else
            {
                AIEnterMoveState(guardPosition, AIManager.movementSpeed);
            }

            stateMachine.ExecuteStateUpdate();
            UpdateAnimator();
        }

        private void AggroBehaviour()
        {
            AIManager.timeSinceAggravated = 0;
            if (!AIManager.WithinAttackRange())
            {
                AIEnterChaseState(AIManager.movementSpeed);
            }
            else
            {
                AIEnterAttackState();
            }
        }

        private void AIEnterIdleState()
        {
            SetAICombatStance(false);
            stateMachine.changeState(new AI_Idler(), StateEnum.Idle);
        }
        private void AIEnterSuspiciousState()
        {
            SetAICombatStance(true);
            stateMachine.changeState(new AI_Idler(), StateEnum.Idle);
        }
        public void AIEnterChaseState(float movementSpeed)
        {
            SetAICombatStance(false);
            stateMachine.changeState(new AI_Mover(player, navMeshAgent, movementSpeed), StateEnum.Chase);
        }
        public void AIEnterMoveState(Vector3 destination, float movementSpeed)
        {
            SetAICombatStance(false);
            stateMachine.changeState(new AI_Mover(destination, navMeshAgent, movementSpeed), StateEnum.Move);
        }
        private void AIEnterAttackState()
        {
            SetAICombatStance(true);
            stateMachine.changeState(new AI_Attacker(player), StateEnum.Attack);
        }

        public void SetAICombatStance(bool value)
        {
            if (animator.GetBool("inCombat") == value) return;
            animator.SetBool("inCombat", value);
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