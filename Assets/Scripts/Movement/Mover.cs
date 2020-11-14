using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] float currentSpeed = 6f;
        [SerializeField] float maxNavPathLength = 50f;

        NavMeshAgent navMeshAgent = null;
        Animator animator = null;
        Dasher dasher = null;

        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            dasher = GetComponent<Dasher>();
        }

        public void InteractWithMovement()
        {
            if (dasher.InteractWithDasher())
            {
                currentSpeed = dasher.GetDashSpeed();
            }
            else
            {
                currentSpeed = 6f;
            }

            // Get movement Input
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            // Detect movement input
            bool shouldMove = Mathf.Abs(movement.x) > Mathf.Epsilon || Mathf.Abs(movement.z) > Mathf.Epsilon;
            if (!shouldMove) return;

            // If there is input, then move
            StartMoveAction(transform.position + movement, 1f);
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = destination;
            navMeshAgent.speed = currentSpeed * Mathf.Clamp01(speedFraction);
        }

        public bool CanDash()
        {
            return dasher.GetCanDash();
        }

        public bool IsDashing()
        {
            return dasher.isDashing;
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();

            // Returns if there is no possible path to destination
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;

            // Returns if there is no possible SINGLE path to destination
            if (path.status != NavMeshPathStatus.PathComplete) return false;

            // Returns if path is TOO long
            if (GetPathLength(path) > maxNavPathLength) return false;
            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            // Returns total path distance
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return total;
        }

        public void Cancel()
        {
            navMeshAgent.speed = 0f;
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
        }
    }
}