using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(menuName = "Abilities/Create New AOE Skill", order = 1)]
    public class AOESkill : SkillScriptableObject
    {
        private UseAOESkill useAOESkill;

        public override void Initialize(GameObject player_GO, string skillType)
        {
            useAOESkill = player_GO.GetComponent<UseAOESkill>();
            useAOESkill.Initialize(player_GO, skillType);
        }

        public override void TriggerSkill()
        {
            useAOESkill.Activate();
        }
    }
}