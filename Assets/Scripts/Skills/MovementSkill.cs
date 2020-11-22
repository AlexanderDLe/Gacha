using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(menuName = "Abilities/Create New Movement Skill", order = 2)]
    public class MovementSkill : SkillScriptableObject
    {
        private UseMovementSkill useMovementSkill;

        public override void Initialize(GameObject player_GO, string skillType)
        {
            useMovementSkill = player_GO.GetComponent<UseMovementSkill>();
            useMovementSkill.Initialize(player_GO, skillType);
        }

        public override void TriggerSkill()
        {
            useMovementSkill.Activate();
        }
    }
}