using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utility
{
    public class DebugObject : MonoBehaviour
    {
        float currentLifetime = 0f;
        float maxLifetime = 10f;

        private void Update()
        {
            currentLifetime += Time.deltaTime;
            if (currentLifetime > maxLifetime)
            {
                ResetScale();
                gameObject.SetActive(false);
            }
        }

        public void Initialize(Vector3 spawnPos, float scale, float maxLifetime)
        {
            gameObject.transform.position = spawnPos;
            gameObject.transform.localScale *= scale;

            this.currentLifetime = 0;
            this.maxLifetime = maxLifetime;
        }

        private void ResetScale()
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}