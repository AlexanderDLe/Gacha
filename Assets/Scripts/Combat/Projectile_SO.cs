using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "ProjectileScriptableObject", menuName = "Projectile/Create New Projectile", order = 0)]
    public class Projectile_SO : ScriptableObject
    {
        public GameObject prefab = null;
        public GameObject hitEffect = null;
        public float speed = 1f;
        public float maxLifeTime = 5f;
    }
}