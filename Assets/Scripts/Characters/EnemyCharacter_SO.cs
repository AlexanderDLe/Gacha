using RPG.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "Character/Create New Enemy Character", order = 1)]
    public class EnemyCharacter_SO : BaseCharacter_SO
    {
        [FoldoutGroup("Attributes")]
        public float movementSpeed;
        [FoldoutGroup("Attributes")]
        public int experienceGiven;

        [FoldoutGroup("Attack")]
        public float attackCooldownTime;
        [FoldoutGroup("Attack")]
        public float weaponRange;
        [FoldoutGroup("Attack")]
        public Projectile_SO projectile_SO;
        [FoldoutGroup("Attack")]
        public float chaseDistance;
    }
}