using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class EffectObject : MonoBehaviour
    {
        float currentTimer = 0;
        float maxLifetime = 5f;

        private void Update()
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > maxLifetime)
            {
                currentTimer = 0;
                gameObject.SetActive(false);
            }
        }

        public void Initialize(Vector3 spawnPos, Quaternion rotation, float maxLifetime)
        {
            gameObject.transform.position = spawnPos;
            gameObject.transform.rotation = rotation;
            this.maxLifetime = maxLifetime;
        }
    }
}