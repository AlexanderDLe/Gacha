using UnityEngine;
using UnityEngine.AI;
using RPG.PlayerStates;
using RPG.Core;
using RPG.Characters;

namespace RPG.Control
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(RaycastMousePosition))]
    [RequireComponent(typeof(PlayerManager), typeof(StateMachine))]
    public class PlayerController : MonoBehaviour
    {
        public static Camera cam = null;
        Animator animator = null;
        NavMeshAgent navMeshAgent = null;
        RaycastMousePosition raycaster = null;
        StateMachine stateMachine = null;
        PlayerManager playerManager = null;
        AimManager aimer = null;

        SkillEventHandler skillEventHandler = null;
        SkillManager primarySkill = null;
        SkillManager ultimateSkill = null;
        SkillManager movementSkill = null;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            stateMachine = GetComponent<StateMachine>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            raycaster = GetComponent<RaycastMousePosition>();
            if (!cam) cam = Camera.main;
        }
        private void Start()
        {
            aimer = playerManager.aimer;
            EnterMovementState();
        }
        private void OnEnable()
        {
            playerManager.CharacterInitializationComplete += UpdateCharacterSkills;
        }
        private void OnDisable()
        {
            playerManager.CharacterInitializationComplete += UpdateCharacterSkills;
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
            skillEventHandler = playerManager.skillEventHandler;
            primarySkill = playerManager.primarySkill;
            ultimateSkill = playerManager.ultimateSkill;
            movementSkill = playerManager.movementSkill;
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
            if (primarySkill.IsAimingEnabled() &&
                playerManager.CanUsePrimarySkill())
            {
                EnterPrimarySkillState();
                return;
            }
            if (movementSkill.IsAimingEnabled() &&
                playerManager.CanUseMovementSkill())
            {
                EnterMovementSkillState();
                return;
            }
            if (ultimateSkill.IsAimingEnabled() &&
                playerManager.CanUseUltimateSkill())
            {
                EnterUltimateSkillState();
                return;
            }
            if (!playerManager.CanAutoAttack()) return;
            playerManager.attacker.SetIsInAutoAttackState(true);
            stateMachine.changeState(new P_AutoAttack(gameObject, animator, raycaster,
            playerManager, repeatAttack), StateEnum.AutoAttack);
        }
        private void HandleRightMouseClick()
        {
            if (!playerManager.CanUseMovementSkill()) return;
            if (movementSkill.SkillRequiresAim() &&
               !movementSkill.IsAimingEnabled())
            {
                playerManager.ActivateSkillAim(movementSkill, "movementSkill");
                return;
            }
            EnterMovementSkillState();
        }
        private void HandlePressE()
        {
            if (!playerManager.CanUsePrimarySkill()) return;
            if (primarySkill.SkillRequiresAim() &&
               !primarySkill.IsAimingEnabled())
            {
                playerManager.ActivateSkillAim(primarySkill, "primarySkill");
                return;
            }
            EnterPrimarySkillState();
        }
        private void HandlePressQ()
        {
            if (!playerManager.CanUseUltimateSkill()) return;
            if (ultimateSkill.SkillRequiresAim() &&
               !ultimateSkill.IsAimingEnabled())
            {
                playerManager.ActivateSkillAim(ultimateSkill, "ultimateSkill");
                return;
            }
            EnterUltimateSkillState();
        }
        private void HandleMovementInput()
        {
            if (!playerManager.CanMove()) return;
            EnterMovementState();
        }
        private void HandleLeftShift()
        {
            if (!playerManager.CanDash()) return;
            playerManager.dasher.TriggerDash();
            EnterDashState();
        }
        private void HandleCharacterSwap(int index)
        {
            if (!playerManager.CanSwapCharacter()) return;
            playerManager.SwapCharacter(index);
        }
        private void HandlePressEscape()
        {
            if (aimer.IsAimingActive()) aimer.CancelAiming();
        }

        private void EnterSwapCharacterState(int index)
        {
            stateMachine.changeState(new P_SwapCharacter(playerManager, index), StateEnum.SwapCharacter);
        }
        private void EnterDashState()
        {
            stateMachine.changeState(new P_Dash(gameObject, navMeshAgent, animator,
            playerManager), StateEnum.Dash);
        }
        private void EnterMovementSkillState()
        {
            stateMachine.changeState(new P_Skill(gameObject, animator, playerManager, movementSkill, skillEventHandler, raycaster), StateEnum.MovementSkill);
        }
        private void EnterPrimarySkillState()
        {
            stateMachine.changeState(new P_Skill(gameObject, animator, playerManager, primarySkill, skillEventHandler, raycaster), StateEnum.PrimarySkill);
        }
        private void EnterUltimateSkillState()
        {
            stateMachine.changeState(new P_Skill(gameObject, animator, playerManager, ultimateSkill, skillEventHandler, raycaster), StateEnum.UltimateSkill);
        }
        private void EnterMovementState()
        {
            stateMachine.changeState(new P_Move(gameObject, navMeshAgent), StateEnum.Move);
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