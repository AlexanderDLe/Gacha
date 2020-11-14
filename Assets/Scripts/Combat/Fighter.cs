using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        ActionScheduler actionScheduler = null;
        NavMeshAgent navMeshAgent = null;
        Animator animator = null;
        AutoAttack autoAttack = null;
        PrimarySkill primarySkill = null;

        [SerializeField] LayerMask enemyLayers;
        private string[] animList = null;

        private void Awake()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            primarySkill = GetComponent<PrimarySkill>();
            autoAttack = GetComponent<AutoAttack>();
        }

        private void Start()
        {
            animList = autoAttack.GetAnimList();
        }

        public bool InteractWithCombat(Vector3 target, bool canDash)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                HandleLeftShift(canDash);
                if (canDash)
                {
                    navMeshAgent.isStopped = false;
                    return false;
                }
            }
            if (Input.GetMouseButtonDown(0)) HandleLeftMouseClick(target);
            if (Input.GetKeyDown(KeyCode.E)) HandleEKeyPress(target);
            if (Input.GetKeyDown(KeyCode.Q)) HandleQKeyPress();

            if (IsCombatAnimationActive())
            {
                navMeshAgent.isStopped = true;
                return true;
            }
            navMeshAgent.isStopped = false;
            return false;
        }

        public void DebugOnLeftShift(string text)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)) Debug.Log(text);
        }

        private void HandleLeftShift(bool canDash)
        {
            // If you can dash then cancel autoattacking & primary skill
            if (canDash) DashCancel();
        }

        public void DashCancel()
        {
            Debug.Log("<color=yellow>Dash Cancel.</color>");
            autoAttack.CancelAutoAttack();
            primarySkill.CancelPrimarySkill();
        }

        private void HandleLeftMouseClick(Vector3 target)
        {
            // If primary skill is still animating, then do not cancel
            if (primarySkill.IsInPrimarySkillAnimation()) return;

            // If primary skill is aiming, then trigger the primary skill
            if (primarySkill.IsAimingPrimarySkill())
            {
                primarySkill.TriggerPrimarySkill(target);
            }
            // If primary skill is not aiming, then trigger attack
            else
            {
                actionScheduler.StartAction(this);
                autoAttack.TriggerAutoAttack(target);
            }
        }

        private void HandleEKeyPress(Vector3 target)
        {
            // If autoattacking, then do not cancel
            if (autoAttack.IsAutoAttacking()) return;
            if (autoAttack.IsInAutoAttackAnimation()) return;
            actionScheduler.StartAction(this);
            primarySkill.TriggerPrimarySkill(target);
        }

        private static void HandleQKeyPress()
        {
            print("Ultimate Skill Activated");
        }

        public bool IsCombatAnimationActive()
        {
            // Will loop through the different animations triggers.
            // TRIGGERS AND STATES must be named exactly the SAME in Animator!
            foreach (string anim in animList)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
                {
                    return true;
                }
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("primarySkill"))
            {
                return true;
            }
            return false;
        }

        public void Cancel()
        {
            // foreach (string anim in animList)
            // {
            //     animator.ResetTrigger(anim);
            // }
            // animator.ResetTrigger("primarySkill");
            // animator.SetTrigger("resetAttack");
        }
    }
}