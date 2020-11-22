using UnityEngine;

namespace RPG.Core
{
    public class UseAOESkill : MonoBehaviour
    {
        RaycastMousePosition raycaster = null;
        Animator animator = null;
        private string skillType;

        public void Initialize(Animator animator, RaycastMousePosition raycaster, string skillType)
        {
            this.raycaster = raycaster;
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