using UnityEngine;

namespace RPG.Combat
{
    public class AOECaster : MonoBehaviour
    {
        ObjectPooler objectPooler = null;

        public void LinkReferences(ObjectPooler objectPooler)
        {
            this.objectPooler = objectPooler;
        }

        public void Cast()
        {
            GameObject VFX = ExtractFromObjectPool("TO DO");
        }

        private GameObject ExtractFromObjectPool(string prefabName)
        {
            return objectPooler.SpawnFromPool(prefabName);
        }
    }
}