using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class AutoAttack : MonoBehaviour
    {
        Animator animator = null;

        [SerializeField] LayerMask enemyLayers;
        [SerializeField] Transform attackHitBoxPoint = default;
        [Range(0f, 2f)]
        [SerializeField] public float attackRange = .5f;

        [Space]
        [Header("Particle Effects")]
        [SerializeField] GameObject[] attackFX = null;

        [Space]
        [Header("Combo Cycle")]
        [SerializeField] string[] animList = { "attack1", "attack2" };
        [SerializeField] float[] damageList = { 5f, 7f };
        [Tooltip("The full time it takes to reset auto attack combo after each attack.")]
        [SerializeField] float comboResetTime = 1f;
        [SerializeField] bool isAutoAttacking = false;

        [Tooltip("The current time in each attack cycle until it reaches reset time.")]
        [SerializeField] float comboTimer = 0f;

        [Tooltip("The current index of the combo.")]
        [SerializeField] int comboNum = 0;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnDrawGizmos()
        {
            if (!attackHitBoxPoint) return;
            Gizmos.color = new Color(1, 0, 0, .3f);
            Gizmos.DrawWireSphere(attackHitBoxPoint.position, attackRange);
        }

        private void Update()
        {
            UpdateAutoAttackCycle();
        }

        public void TriggerAutoAttack(Vector3 target)
        {
            transform.LookAt(target);
            if (Input.GetButtonDown("Fire1") && comboNum < animList.Length)
            {
                // If there is an active attack animation and the countdown is still active, you can't attack
                if (isAutoAttacking) return;
                // GetComponent<ActionScheduler>().StartAction(this);
                animator.SetTrigger(animList[comboNum]);
                comboNum++;
                comboTimer = 0f;
            }
        }

        public void UpdateAutoAttackCycle()
        {
            if (comboNum > 0)
            {
                comboTimer += Time.deltaTime;
                if (comboTimer > comboResetTime)
                {
                    ResetAttacks();
                    comboNum = 0;
                }
                if (comboNum == animList.Length)
                {
                    comboNum = 0;
                }
            }
        }

        public void CancelAutoAttack()
        {
            ResetAttacks();
            isAutoAttacking = false;
        }

        private void ResetAttacks()
        {
            foreach (string anim in animList)
            {
                animator.ResetTrigger(anim);
            }
            animator.SetTrigger("resetAttack");
        }

        public string[] GetAnimList()
        {
            return animList;
        }

        public bool IsAutoAttacking()
        {
            return isAutoAttacking;
        }

        public bool IsInAutoAttackAnimation()
        {
            foreach (string anim in animList)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
                {
                    return true;
                }
            }
            return false;
        }

        private void InflictDamageHitbox(int comboIndex)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackHitBoxPoint.position, attackRange, enemyLayers);

            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<Health>().TakeDamage(damageList[comboIndex]);
            }
            isAutoAttacking = false;
        }

        // Animator Triggered Events
        private void AttackStart()
        {
            isAutoAttacking = true;
        }

        private void Attack1()
        {
            InflictDamageHitbox(0);
            Instantiate(attackFX[0], attackHitBoxPoint.position, gameObject.transform.rotation);
        }

        private void Attack2()
        {
            InflictDamageHitbox(1);
            Instantiate(attackFX[1], attackHitBoxPoint.position, gameObject.transform.rotation);
        }
    }
}
