using UnityEngine;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        public Transform hitboxPoint;
        public float hitboxDebugRadius;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitboxPoint.position, hitboxDebugRadius);
        }
    }
}