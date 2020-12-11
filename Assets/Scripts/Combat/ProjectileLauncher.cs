using UnityEngine;

namespace RPG.Combat
{
    public class ProjectileLauncher : MonoBehaviour
    {
        ObjectPooler objectPooler = null;

        public void LinkReferences(ObjectPooler objectPooler)
        {
            this.objectPooler = objectPooler;
        }

        public void Shoot(string prefabName, Vector3 origin, Vector3 destination, float speed, float damage, float lifetime, LayerMask layerToHarm)
        {
            Projectile proj = ExtractFromObjectPool(prefabName);

            proj.Initialize(origin, destination, speed, damage, lifetime, layerToHarm);
            proj.transform.LookAt(destination);
        }

        public void Shoot(string prefabName, Vector3 origin, Vector3 destination, float speed, float damage, float lifetime, LayerMask layerToHarm, bool hasActiveLifetime, float activeLifetime)
        {
            Projectile proj = ExtractFromObjectPool(prefabName);

            proj.Initialize(origin, destination, speed, damage, lifetime, layerToHarm, hasActiveLifetime, activeLifetime);
            proj.transform.LookAt(destination);
        }

        private Projectile ExtractFromObjectPool(string prefabName)
        {
            return objectPooler.SpawnFromPool(prefabName).GetComponent<Projectile>();
        }
    }
}