using RPG.AI;
using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class AOEInvoker : MonoBehaviour
    {
        ObjectPooler objectPooler = null;

        public void LinkReferences(ObjectPooler objectPooler)
        {
            this.objectPooler = objectPooler;
        }

        public void Invoke(Vector3 hitboxPosition, float radius, LayerMask layer, float damage)
        {
            Collider[] hits = Physics.OverlapSphere(hitboxPosition, radius, layer);

            foreach (Collider hit in hits)
            {
                BaseManager target = hit.GetComponent<BaseManager>();
                target.TakeDamage((int)damage);
            }
        }
    }
}