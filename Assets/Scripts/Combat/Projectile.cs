using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        private float speed = 0f;
        private Vector3 destination = Vector3.zero;

        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void Initialize(float speed, Vector3 destination)
        {
            this.speed = speed;
            this.destination = destination;
            transform.LookAt(destination);
        }
    }
}