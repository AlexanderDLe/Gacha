using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class DashManager : MonoBehaviour
    {
        public float dashSpeed = 20f;
        public int maxDashCharges = 3;
        public float dashRegenRate = 3f;
        public bool isDashing = false;
        public int currentDashCharges = 3;

        private void Start()
        {
            currentDashCharges = maxDashCharges;
        }

        public bool GetCanDash()
        {
            return currentDashCharges > 0;
        }

        public bool IsDashing()
        {
            return isDashing;
        }

        public void SetIsDashing(bool value)
        {
            isDashing = value;
        }

        public float GetDashSpeed()
        {
            return dashSpeed;
        }

        public void TriggerDash()
        {
            isDashing = true;
            StartCoroutine(RegenDashCharge());
        }

        public event Action OnDashUpdate;
        IEnumerator RegenDashCharge()
        {
            currentDashCharges--;
            OnDashUpdate();
            yield return new WaitForSeconds(dashRegenRate);
            currentDashCharges++;
            OnDashUpdate();
        }
    }
}