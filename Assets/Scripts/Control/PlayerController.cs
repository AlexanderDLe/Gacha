using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        public static Camera cam = null;
        NavMeshAgent navMeshAgent = null;
        Animator animator = null;
        Mover mover = null;
        Fighter fighter = null;

        RaycastMousePosition raycaster = null;
        RaycastHit ray = default;


        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            raycaster = GetComponent<RaycastMousePosition>();
            if (!cam) cam = Camera.main;
        }

        void Update()
        {
            ray = raycaster.GetRaycastMousePoint();

            if (!mover.IsDashing())
            {
                if (fighter.InteractWithCombat(ray.point, mover.CanDash())) return;
            }
            mover.InteractWithMovement();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }
    }
}