using RPG.AI;
using RPG.Attributes;
using RPG.Control;
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

            if (layer == LayerMask.GetMask("Enemy")) AffectEnemies(damage, hits);
            if (layer == LayerMask.GetMask("Player")) AffectPlayer(damage, hits);
        }

        private static void AffectPlayer(float damage, Collider[] hits)
        {
            foreach (Collider hit in hits)
            {
                BaseStats player = hit.GetComponent<StateManager>().baseStats;
                player.TakeDamage((int)damage);
            }
        }

        private static void AffectEnemies(float damage, Collider[] hits)
        {
            foreach (Collider hit in hits)
            {
                AIManager AIEnemy = hit.GetComponent<AIManager>();
                AIEnemy.TakeDamage((int)damage);
            }
        }
    }
}