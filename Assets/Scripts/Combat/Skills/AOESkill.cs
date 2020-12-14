using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using RPG.Combat;

namespace RPG.Skill
{
    public enum AOETargetEnum
    {
        None,
        Self,
        MousePosition,
        ProvidedByPrefab,
        TargetPlayer,
        SphereCastToPoint
    }
    [CreateAssetMenu(menuName = "Abilities/Create New AOE Skill", order = 1)]
    public class AOESkill : Skill_SO
    {
        [FoldoutGroup("AOE"), Title("AOE Targeting")]
        [FoldoutGroup("AOE"), InfoBox("If you use ProvidedByPrefab, you must 1 - Attach a 'AOEEffect' script to the skill prefab. 2 - Provide a Transform in the prefab to the AOECenterPoint variable on a AOEEffect script.", "ShowAOETargetProvidedInfoBox")]
        [FoldoutGroup("AOE")]
        public AOETargetEnum aoeTargetEnum;

        bool ShowAOETargetProvidedInfoBox()
        {
            return aoeTargetEnum == AOETargetEnum.ProvidedByPrefab;
        }

        [FoldoutGroup("AOE")]
        public float radius = 1;
        [FoldoutGroup("AOE"), ShowIf("aoeTargetEnum", AOETargetEnum.SphereCastToPoint)]
        public float distance = 1;

        [Space, Title("AOE Effect Section")]
        [FoldoutGroup("AOE")]
        public bool repeatChain;
        [FoldoutGroup("AOE"), ShowIf("repeatChain")]
        public float repeatDelay = 3f;
        [FoldoutGroup("AOE")]
        public List<AOEPackageItem> aoePackageChain;
    }
}
