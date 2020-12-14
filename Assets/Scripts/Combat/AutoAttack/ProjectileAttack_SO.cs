using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(menuName = "AutoAttack/Create New Projectile Attack", order = 0)]
    public class ProjectileAttack_SO : AutoAttack_SO
    {
        [FoldoutGroup("Metadata")]
        public Projectile_SO projectile;
    }
}