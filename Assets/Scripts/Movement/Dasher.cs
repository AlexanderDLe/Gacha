using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Movement
{
    public class Dasher : MonoBehaviour, IAction
    {
        Animator animator = null;
        [SerializeField] GameObject dashParticleFX = null;
        [SerializeField] float dashSpeed = 10f;
        [SerializeField] bool canDash = true;
        public bool isDashing = false;

        [Header("Dash Charges")]
        [SerializeField] int maxDashCharges = 3;
        [SerializeField] int currentDashCharges = 3;

        [Header("Dash Regen")]
        [SerializeField] float dashRegenRate = 3f;
        [SerializeField] float dashRegenTimer = 0;

        // [Space]
        // [Tooltip("The dash length.")]
        // [SerializeField] float dashLengthTime = .5f;
        // [Tooltip("The current time in the dash cycle.")]
        // [SerializeField] float dashTimer = 0f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            currentDashCharges = maxDashCharges;
        }

        void Update()
        {
            if (currentDashCharges < maxDashCharges)
            {
                dashRegenTimer += Time.deltaTime;
                if (dashRegenTimer >= dashRegenRate)
                {
                    dashRegenTimer = 0;
                    currentDashCharges++;
                }
            }
            if (!isDashing && currentDashCharges > 0)
            {
                canDash = true;
            }
        }

        public bool InteractWithDasher()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing) TriggerDash();
            if (isDashing) return true;
            return false;
        }

        private void TriggerDash()
        {
            if (currentDashCharges == 0) return;
            // Debug.Log("<color=yellow>Should Dash</color>");
            currentDashCharges--;
            isDashing = true;
            canDash = false;
            animator.SetTrigger("dash");
        }

        public float GetDashSpeed()
        {
            return dashSpeed;
        }
        public bool GetCanDash()
        {
            return canDash;
        }

        public void Cancel() { }
        private void StartDash()
        {
            Debug.Log("<color=white>Start Dash.</color>");
            if (dashParticleFX) Instantiate(dashParticleFX, transform.position, transform.rotation);
        }
        private void EndDash()
        {
            Debug.Log("<color=green>End Dash.</color>");
            isDashing = false;
        }


    }
}