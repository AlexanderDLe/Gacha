using RPG.Core;
using UnityEngine;

namespace RPG.Skill
{
    public enum AOETargetEnum
    {
        Default,
        Self,
        MousePosition
    }
    [CreateAssetMenu(menuName = "Abilities/Create New AOE Skill", order = 1)]
    public class AOESkill : Skill_SO
    {
        public AOETargetEnum aoeTargetEnum;
    }
}