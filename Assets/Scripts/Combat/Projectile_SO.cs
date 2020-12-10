using Sirenix.OdinInspector;
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


        [FoldoutGroup("Active Lifetime")]
        [InfoBox("In cases where you want to disable the active hitbox before you want to completely disable the game object projectile, you can use Active Time. This can be especially useful, for example, when you want to allow particles to linger for a while longer after a hitbox is disabled.")]
        public bool hasActiveTime = false;

        [FoldoutGroup("Active Lifetime"), ShowIf("hasActiveTime")]
        public float activeTime = 5f;
    }
}