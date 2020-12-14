using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(menuName = "AutoAttack/Create New Melee Attack", order = 0)]
    public class MeleeAttack_SO : AutoAttack_SO
    {
        [FoldoutGroup("Attack")]
        public float[] autoAttackHitRadiuses;
    }
}