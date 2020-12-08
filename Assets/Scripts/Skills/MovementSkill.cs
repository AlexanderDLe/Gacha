using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(menuName = "Abilities/Create New Movement Skill", order = 2)]
    public class MovementSkill : Skill_SO
    {
        RaycastMousePosition raycaster = null;
        Animator animator = null;
        GameObject player = null;

        public override void Initialize(GameObject player_GO, string skillType)
        {
            this.raycaster = player_GO.GetComponent<RaycastMousePosition>();
            this.animator = player_GO.GetComponent<Animator>();
            this.player = player_GO;
        }

        public override void TriggerSkill(string skillType)
        {
            raycaster.RotateObjectTowardsMousePosition(player);
            // Debug.Log("After Raycast Rotation: " + player.transform.rotation);
            animator.SetTrigger(skillType);
        }
    }
}