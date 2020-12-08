using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(menuName = "Abilities/Create New AOE Skill", order = 1)]
    public class AOESkill : Skill_SO
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
            animator.SetTrigger(skillType);
        }
    }
}