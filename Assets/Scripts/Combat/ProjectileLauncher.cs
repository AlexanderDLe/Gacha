using UnityEngine;

namespace RPG.Combat
{
    public class ProjectileLauncher : MonoBehaviour
    {
        ObjectPooler objectPooler = null;

        private void Awake()
        {
            objectPooler = GameObject.FindWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        }

        public void Shoot(string prefabName, Vector3 origin, Vector3 destination, float speed, float damage, float lifetime, string layerToHarm)
        {
            Projectile proj = objectPooler.SpawnFromPool(prefabName).GetComponent<Projectile>();

            proj.Initialize(origin, destination, speed, damage, lifetime, layerToHarm);
        }
    }
}