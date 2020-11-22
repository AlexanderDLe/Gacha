using UnityEngine;

namespace RPG.Core
{
    public class UseProjectileSkill : MonoBehaviour
    {
        RaycastMousePosition raycaster = null;
        Animator animator = null;
        private string skillType;

        public void Initialize(GameObject player_GO, string skillType)
        {
            this.raycaster = player_GO.GetComponent<RaycastMousePosition>();
            this.animator = player_GO.GetComponent<Animator>();
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