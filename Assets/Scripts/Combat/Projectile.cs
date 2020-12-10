﻿using RPG.Attributes;
using RPG.Control;
using UnityEngine;
using RPG.AI;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        Vector3 projectileHeightAdjustment = new Vector3(0, .6f, 0);
        Vector3 destination = Vector3.zero;
        float speed = 0f;
        float damage = 0f;
        string tagToHarm;
        float projectileLifetime = 5f;
        public float currentLifetime = 0f;
        string layerToHarm;

        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            UpdateLifetimeCountdown();
        }

        private void UpdateLifetimeCountdown()
        {
            currentLifetime += Time.deltaTime;
            if (currentLifetime > projectileLifetime)
            {
                gameObject.SetActive(false);
            }
        }

        public void Initialize(Vector3 spawnPos, Vector3 destination, float speed, float damage, float projectileLifetime, string layerToHarm)
        {
            this.destination = destination;
            transform.position = spawnPos;
            transform.LookAt(destination + projectileHeightAdjustment);

            this.speed = speed;
            this.damage = damage;

            this.currentLifetime = 0;
            this.projectileLifetime = projectileLifetime;

            this.layerToHarm = layerToHarm;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(layerToHarm))
            {
                if (layerToHarm == "Player")
                {
                    BaseStats target = null;
                    target = other.gameObject.GetComponent<StateManager>().baseStats;
                    target.TakeDamage(damage);
                }
                else if (layerToHarm == "Enemy")
                {
                    AIManager target = null;
                    target = other.gameObject.GetComponent<AIManager>();
                    target.TakeDamage((int)damage);
                }
            }
            // if (other.CompareTag(tagToHarm))
            // {
            //     if (tagToHarm == "Player")
            //     {
            //         BaseStats target = null;
            //         target = other.gameObject.GetComponent<StateManager>().baseStats;
            //         target.TakeDamage(damage);
            //     }
            //     else if (tagToHarm == "Enemy")
            //     {
            //         AIManager target = null;
            //         target = other.gameObject.GetComponent<AIManager>();
            //         target.TakeDamage((int)damage);
            //     }
            // }
        }
    }
}