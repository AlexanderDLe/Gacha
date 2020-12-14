using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(menuName = "AutoAttack/Create New Projectile Attack", order = 0)]
    public class ProjectileAttack_SO : AutoAttack_SO
    {
        public FightTypeEnum fightingType = FightTypeEnum.Projectile;
        public Projectile_SO projectile;
    }
}