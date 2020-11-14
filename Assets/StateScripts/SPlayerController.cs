using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SPlayerController : MonoBehaviour
    {
        StateMachine stateMachine = null;
        public static Camera cam = null;
        NavMeshAgent navMeshAgent = null;
        Animator animator = null;
        StateManager stateManager = null;

        private void Awake()
        {
            stateMachine = gameObject.AddComponent(typeof(StateMachine)) as StateMachine;
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            stateManager = GetComponent<StateManager>();
            if (!cam) cam = Camera.main;
        }

        private void Start()
        {
            stateMachine.changeState(new SMover(gameObject, navMeshAgent));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)) HandleLeftShift();
            if (Input.GetMouseButtonDown(0)) HandleLeftMouseClick();
            stateMachine.ExecuteStateUpdate();
            UpdateAnimator();
        }

        private void HandleLeftMouseClick()
        {
            if (stateManager.GetIsDashing()) return;
        }

        private void HandleLeftShift()
        {
            if (stateManager.CanDash())
            {
                stateManager.TriggerDash();
                stateMachine.changeState(new SDasher(gameObject, navMeshAgent, animator, stateManager.GetDashSpeed()));
            }
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }

        public void StartDash() { }

        public void EndDash()
        {
            stateManager.SetIsDashing(false);
            stateMachine.changeState(new SMover(gameObject, navMeshAgent));
        }
    }
}