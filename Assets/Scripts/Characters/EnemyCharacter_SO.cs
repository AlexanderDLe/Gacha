using RPG.Combat;
using RPG.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "Character/Create New Enemy Character", order = 1)]
    public class EnemyCharacter_SO : BaseCharacter_SO
    {
        [FoldoutGroup("Attributes")]
        public int experienceGiven;
        [FoldoutGroup("Attributes")]
        public Rarity EnemyRarity;

        [FoldoutGroup("Attack")]
        public FightingType fightingType;
        [FoldoutGroup("Attack"), ShowIf("fightingType", FightingType.Projectile)]
        public Projectile_SO projectile_SO;
        [FoldoutGroup("Attack"), ShowIf("fightingType", FightingType.Melee)]
        public float hitboxRadius;
        [FoldoutGroup("Attack")]
        public Weapon weapon;
        [FoldoutGroup("Attack")]
        public float attackCooldownTime;
        [FoldoutGroup("Attack")]
        public float weaponRange;
        [FoldoutGroup("Attack")]
        public float chaseDistance;
        [FoldoutGroup("Attack")]
        public float aggroCooldownTime;
        [FoldoutGroup("Attack")]
        public float suspicionTime;

    }
}