using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using RPG.Combat;

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
        [FoldoutGroup("AOE")]
        public AOETargetEnum aoeTargetEnum;

        [FoldoutGroup("AOE")]
        public bool repeatChain;
        [FoldoutGroup("AOE")]
        public List<Effect> effectChain;
    }
}
