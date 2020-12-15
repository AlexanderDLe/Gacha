using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(menuName = "Abilities/Create New Projectile Skill", order = 0)]
    public class ProjectileSkill : Skill_SO
    {
        [FoldoutGroup("Projectile")]
        public Projectile_SO projectile_SO = null;

        [FoldoutGroup("Projectile")]
        public EffectPackage effectPackage;
    }
}