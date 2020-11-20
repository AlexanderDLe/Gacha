using UnityEngine;

namespace RPG.Core
{
    public class UseProjectileSkill : MonoBehaviour
    {
        RaycastMousePosition raycaster = null;
        Animator animator = null;
        private string skillType;

        public void Initialize(Animator animator, string skillType)
        {
            this.raycaster = GetComponent<RaycastMousePosition>();
            this.animator = animator;
            this.skillType = skillType;
        }

        public void Activate()
        {
            RaycastHit hit = raycaster.GetRaycastMousePoint();
            gameObject.transform.LookAt(hit.point);
            animator.SetTrigger(skillType);
        }
    }
}