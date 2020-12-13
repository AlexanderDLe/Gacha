using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class E_Damage : IEffect
    {
        Collider[] hits;
        float damage;
        float radius;
        LayerMask layer;
        Vector3 hitPos;

        public E_Damage(Collider[] hits, float damage)
        {
            this.hits = hits;
            this.damage = damage;
        }
        public E_Damage(Vector3 hitPos, float radius, LayerMask layer, float damage)
        {
            this.hits = Physics.OverlapSphere(hitPos, radius, layer);
            this.hitPos = hitPos;
            this.damage = damage;
            this.radius = radius;
            this.layer = layer;
        }

        public void ApplyEffect()
        {
            foreach (Collider hit in hits)
            {
                BaseManager target = hit.GetComponent<BaseManager>();
                target.TakeDamage((int)damage);
            }
        }
    }
}