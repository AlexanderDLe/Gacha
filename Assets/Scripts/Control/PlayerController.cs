using RPG.Core;
using RPG.Movement;
using RPG.Combat;
using UnityEngine;
using UnityEngine.AI;
using System;

namespace RPG.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(RaycastMousePosition))]
    [RequireComponent(typeof(StateManager), typeof(StateMachine))]
    public class PlayerController : MonoBehaviour
    {
        public static Camera cam = null;
        Animator animator = null;
        NavMeshAgent navMeshAgent = null;
        RaycastMousePosition raycaster = null;
        StateMachine stateMachine = null;
        StateManager stateManager = null;

        private void Awake()
        {
            stateManager = GetComponent<StateManager>();
            stateMachine = GetComponent<StateMachine>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            raycaster = GetComponent<RaycastMousePosition>();
            if (!cam) cam = Camera.main;
        }

        private void Start()
        {
            EnterMovementState();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)) HandleLeftShift();
            if (Input.GetMouseButtonDown(0)) HandleLeftMouseClick();
            if (Input.GetMouseButtonDown(1)) HandleRightMouseClick();
            if (Input.GetKeyDown(KeyCode.E)) HandlePressE();
            if (DetectMovementInput()) HandleMovementInput();
            stateMachine.ExecuteStateUpdate();
            UpdateAnimator();
        }

        private void HandleRightMouseClick()
        {
            if (!stateManager.CanUseMovementSkill()) return;
            stateManager.MovementSkillTriggered();
            stateMachine.changeState(new MovementSkill(gameObject, animator, raycaster,
            navMeshAgent, stateManager), StateEnum.MovementSkill);
        }

        private void HandlePressE()
        {
            if (!stateManager.CanUsePrimarySkill()) return;
            if (stateManager.PrimarySkillRequiresAim() &&
               !stateManager.GetPrimaryAimingEnabled())
            {
                stateManager.SetPrimaryAimingEnabled(true);
                return;
            }
            EnterPrimarySkillState();
        }

        private void HandleLeftMouseClick()
        {
            if (stateManager.GetPrimaryAimingEnabled())
            {
                EnterPrimarySkillState();
                return;
            }
            if (!stateManager.CanAutoAttack()) return;
            stateManager.SetIsInAutoAttackState(true);
            stateMachine.changeState(new AutoAttack(gameObject, animator, raycaster,
            stateManager), StateEnum.AutoAttack);
        }

        private void HandleMovementInput()
        {
            if (!stateManager.CanMove()) return;
            stateMachine.changeState(new Mover(gameObject, navMeshAgent), StateEnum.Move);
        }

        private void HandleLeftShift()
        {
            if (!stateManager.CanDash()) return;
            stateManager.TriggerDash();
            stateMachine.changeState(new Dasher(gameObject, navMeshAgent, animator,
            stateManager), StateEnum.Dash);
        }

        private void EnterPrimarySkillState()
        {
            stateManager.PrimarySkillTriggered();
            stateMachine.changeState(new PrimarySkill(gameObject, animator, raycaster,
            stateManager), StateEnum.PrimarySkill);
        }

        private void EnterMovementState()
        {
            stateMachine.changeState(new Mover(gameObject, navMeshAgent), StateEnum.Move);
        }

        private bool DetectMovementInput()
        {
            if (
                Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.D))
            {
                return true;
            }
            return false;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }

        public void DashEnd()
        {
            EnterMovementState();
        }
        public void MovementSkillEnd()
        {
            EnterMovementState();
        }
        public void PrimarySkillEnd()
        {
            EnterMovementState();
        }
    }
}