using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class ProjectileLauncher : MonoBehaviour
    {
        public void Launch(Stats stats, ObjectPooler charObjectPooler, Projectile_SO script, Vector3 origin, Vector3 destination, LayerMask layerToHarm, EffectPackage effectPackage)
        {
            string prefabName = script.prefab.name;
            float speed = script.speed;
            float lifetime = script.maxLifeTime;
            float activeTime = script.activeTime;
            bool hasActiveTime = script.hasActiveTime;

            Projectile proj = charObjectPooler.SpawnFromPool(prefabName).GetComponent<Projectile>();

            proj.Initialize(stats, origin, destination, speed, effectPackage, lifetime, layerToHarm, hasActiveTime, activeTime);
        }
    }
}