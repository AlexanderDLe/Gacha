using RPG.AI;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class MeleeAttacker : MonoBehaviour
    {
        public void Strike(Vector3 hitboxPosition, float radius, LayerMask layer, float damage)
        {
            Collider[] hits = Physics.OverlapSphere(hitboxPosition, radius, layer);

            if (layer == LayerMask.GetMask("Enemy")) HitEnemies(damage, hits);
            if (layer == LayerMask.GetMask("Player")) HitPlayer(damage, hits);
        }

        private static void HitPlayer(float damage, Collider[] hits)
        {
            foreach (Collider hit in hits)
            {
                BaseStats player = hit.GetComponent<StateManager>().baseStats;
                player.TakeDamage((int)damage);
            }
        }

        private static void HitEnemies(float damage, Collider[] hits)
        {
            foreach (Collider hit in hits)
            {
                AIManager AIEnemy = hit.GetComponent<AIManager>();
                AIEnemy.TakeDamage((int)damage);
            }
        }
    }
}