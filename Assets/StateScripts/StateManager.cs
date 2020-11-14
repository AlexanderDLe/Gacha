using System.Collections;
using UnityEngine;

namespace RPG.Control
{
    public class StateManager : MonoBehaviour
    {
        #region Dash Mechanics
        [SerializeField] float dashSpeed = 20f;
        public bool isDashing = false;

        [Header("Dash Charges")]
        [SerializeField] int maxDashCharges = 3;
        [SerializeField] int currentDashCharges = 3;

        [Header("Dash Regen")]
        [SerializeField] float dashRegenRate = 3f;
        [SerializeField] float dashRegenTimer = 0;

        public bool GetCanDash()
        {
            return currentDashCharges > 0;
        }

        public bool GetIsDashing()
        {
            return isDashing;
        }

        public bool CanDash()
        {
            if (currentDashCharges > 0 && !isDashing) return true;
            return false;
        }

        public void SetIsDashing(bool value)
        {
            isDashing = value;
        }

        public void TriggerDash()
        {
            isDashing = true;
            StartCoroutine(RegenDashCharge());
        }

        public float GetDashSpeed()
        {
            return dashSpeed;
        }

        IEnumerator RegenDashCharge()
        {
            currentDashCharges--;
            yield return new WaitForSeconds(dashRegenRate);
            currentDashCharges++;
        }
        #endregion

        #region Auto Attack Mechanics
        // [SerializeField] LayerMask enemyLayers;
        // [SerializeField] Transform attackHitBoxPoint = default;
        [Space]
        [Header("Auto Attack")]
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

        [Tooltip("The current index of the combo.")]
        [SerializeField] int comboNum = 0;

        public bool CanAutoAttack()
        {
            if (isDashing) return false;
            return true;
        }
        #endregion
    }
}
