using RPG.Core;
using RPG.Movement;
using RPG.Combat;
using UnityEngine;
using UnityEngine.AI;
using System;
using RPG.Control;

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

        SkillManager primarySkill = null;
        SkillManager ultimateSkill = null;
        SkillManager movementSkill = null;

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
            stateManager.OnCharacterInitialization += UpdateCharacterSkills;
        }
        void Update()
        {
            RepeatAction();

            if (Input.GetKeyDown(KeyCode.LeftShift)) HandleLeftShift();
            if (Input.GetMouseButtonDown(0)) HandleLeftMouseClick();
            if (Input.GetMouseButtonDown(1)) HandleRightMouseClick();
            if (Input.GetKeyDown(KeyCode.E)) HandlePressE();
            if (Input.GetKeyDown(KeyCode.Q)) HandlePressQ();
            if (DetectMovementInput()) HandleMovementInput();
            stateMachine.ExecuteStateUpdate();
            UpdateAnimator();
        }

        public void UpdateCharacterSkills()
        {
            primarySkill = stateManager.primarySkill;
            ultimateSkill = stateManager.ultimateSkill;
            movementSkill = stateManager.movementSkill;
        }

        public bool repeatAttack = false;
        public bool repeatMovement = false;
        public bool repeatPrimary = false;
        public bool repeatUltimate = false;
        public void RepeatAction()
        {
            if (repeatAttack || repeatMovement || repeatPrimary || repeatUltimate)
            {
                if (repeatAttack) HandleLeftMouseClick();
                if (repeatMovement) HandleRightMouseClick();
                if (repeatPrimary) HandlePressE();
                if (repeatUltimate) HandlePressQ();
            }
        }

        private void HandleLeftMouseClick()
        {
            if (stateManager.GetAimingEnabled(primarySkill) &&
                stateManager.CanUsePrimarySkill())
            {
                EnterPrimarySkillState();
                return;
            }
            if (stateManager.GetAimingEnabled(movementSkill) &&
                stateManager.CanUseMovementSkill())
            {
                EnterMovementSkillState();
                return;
            }
            if (stateManager.GetAimingEnabled(ultimateSkill) &&
                stateManager.CanUseUltimateSkill())
            {
                EnterUltimateSkillState();
                return;
            }
            if (!stateManager.CanAutoAttack()) return;
            stateManager.SetIsInAutoAttackState(true);
            stateMachine.changeState(new S_AutoAttack(gameObject, animator, raycaster,
            stateManager, repeatAttack), StateEnum.AutoAttack);
        }
        private void HandleRightMouseClick()
        {
            if (!stateManager.CanUseMovementSkill()) return;
            if (stateManager.GetSkillRequiresAim(movementSkill) &&
               !!stateManager.GetAimingEnabled(movementSkill))
            {
                stateManager.ActivateSkillAim(movementSkill, "movementSkill");
                return;
            }
            EnterMovementSkillState();
        }
        private void HandlePressE()
        {
            if (!stateManager.CanUsePrimarySkill()) return;
            if (stateManager.GetSkillRequiresAim(primarySkill) &&
               !stateManager.GetAimingEnabled(primarySkill))
            {
                stateManager.ActivateSkillAim(primarySkill, "primarySkill");
                return;
            }
            EnterPrimarySkillState();
        }
        private void HandlePressQ()
        {
            if (!stateManager.CanUseUltimateSkill()) return;
            if (stateManager.GetSkillRequiresAim(ultimateSkill) &&
               !stateManager.GetAimingEnabled(ultimateSkill))
            {
                stateManager.ActivateSkillAim(ultimateSkill, "ultimateSkill");
                return;
            }
            EnterUltimateSkillState();
        }
        private void HandleMovementInput()
        {
            if (!stateManager.CanMove()) return;
            EnterMovementState();
        }
        private void HandleLeftShift()
        {
            if (!stateManager.CanDash()) return;
            stateManager.TriggerDash();
            EnterDashState();
        }

        private void EnterDashState()
        {
            stateMachine.changeState(new S_Dasher(gameObject, navMeshAgent, animator,
                        stateManager), StateEnum.Dash);
        }
        private void EnterMovementSkillState()
        {
            stateMachine.changeState(new S_MovementSkill(gameObject, animator, raycaster,
            navMeshAgent, stateManager, movementSkill), StateEnum.MovementSkill);
        }
        private void EnterPrimarySkillState()
        {
            stateMachine.changeState(new S_PrimarySkill(gameObject, animator, raycaster,
            stateManager, primarySkill), StateEnum.PrimarySkill);
        }
        private void EnterUltimateSkillState()
        {
            stateMachine.changeState(new S_UltimateSkill(gameObject, animator, raycaster,
            stateManager, ultimateSkill), StateEnum.UltimateSkill);
        }
        private void EnterMovementState()
        {
            stateMachine.changeState(new S_Mover(gameObject, navMeshAgent), StateEnum.Move);
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

        #region Animator Events
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
        public void UltimateSkillEnd()
        {
            EnterMovementState();
        }
        #endregion
    }
}