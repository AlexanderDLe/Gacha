using RPG.Combat;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "Character/Create New Enemy Character", order = 1)]
    public class EnemyCharacter_SO : BaseCharacter_SO
    {
        public float movementSpeed;
        public float attackCooldown;
        public float weaponRange;
        public Projectile_SO projectile_SO;
    }
}