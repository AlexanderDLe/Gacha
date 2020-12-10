using UnityEngine;
using UnityEngine.AI;
using RPG.PlayerStates;
using RPG.Core;
using RPG.Combat;

namespace RPG.Control
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
        AimManager aimer = null;

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
            aimer = stateManager.aimer;
            EnterMovementState();
        }
        private void OnEnable()
        {
            stateManager.CharacterInitializationComplete += UpdateCharacterSkills;
        }
        private void OnDisable()
        {
            stateManager.CharacterInitializationComplete += UpdateCharacterSkills;
        }

        void Update()
        {
            RepeatAction();

            if (Input.GetKeyDown(KeyCode.LeftShift)) HandleLeftShift();
            if (Input.GetMouseButtonDown(0)) HandleLeftMouseClick();
            if (Input.GetMouseButtonDown(1)) HandleRightMouseClick();
            if (Input.GetKeyDown(KeyCode.E)) HandlePressE();
            if (Input.GetKeyDown(KeyCode.Q)) HandlePressQ();
            if (Input.GetKeyDown(KeyCode.Alpha1)) HandleCharacterSwap(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) HandleCharacterSwap(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) HandleCharacterSwap(2);
            if (Input.GetKeyDown(KeyCode.Escape)) HandlePressEscape();
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
            if (primarySkill.GetAimingEnabled() &&
                stateManager.CanUsePrimarySkill())
            {
                EnterPrimarySkillState();
                return;
            }
            if (movementSkill.GetAimingEnabled() &&
                stateManager.CanUseMovementSkill())
            {
                EnterMovementSkillState();
                return;
            }
            if (ultimateSkill.GetAimingEnabled() &&
                stateManager.CanUseUltimateSkill())
            {
                EnterUltimateSkillState();
                return;
            }
            if (!stateManager.CanAutoAttack()) return;
            stateManager.attacker.SetIsInAutoAttackState(true);
            stateMachine.changeState(new S_AutoAttack(gameObject, animator, raycaster,
            stateManager, repeatAttack), StateEnum.AutoAttack);
        }
        private void HandleRightMouseClick()
        {
            if (!stateManager.CanUseMovementSkill()) return;
            if (movementSkill.SkillRequiresAim() &&
               !movementSkill.GetAimingEnabled())
            {
                stateManager.ActivateSkillAim(movementSkill, "movementSkill");
                return;
            }
            EnterMovementSkillState();
        }
        private void HandlePressE()
        {
            if (!stateManager.CanUsePrimarySkill()) return;
            if (primarySkill.SkillRequiresAim() &&
               !primarySkill.GetAimingEnabled())
            {
                stateManager.ActivateSkillAim(primarySkill, "primarySkill");
                return;
            }
            EnterPrimarySkillState();
        }
        private void HandlePressQ()
        {
            if (!stateManager.CanUseUltimateSkill()) return;
            if (ultimateSkill.SkillRequiresAim() &&
               !ultimateSkill.GetAimingEnabled())
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
            stateManager.dasher.TriggerDash();
            EnterDashState();
        }
        private void HandleCharacterSwap(int index)
        {
            if (!stateManager.CanSwapCharacter()) return;
            stateManager.SwapCharacter(index);
        }
        private void HandlePressEscape()
        {
            if (aimer.IsAimingActive()) aimer.CancelAiming();
        }

        private void EnterSwapCharacterState(int index)
        {
            stateMachine.changeState(new S_SwapCharacter(stateManager, index), StateEnum.SwapCharacter);
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
        public void MovementSkillTriggered()
        {
            EnterMovementState();
        }
        public void PrimarySkillTriggered()
        {
            EnterMovementState();
        }
        public void UltimateSkillTriggered()
        {
            EnterMovementState();
        }
        #endregion
    }
}