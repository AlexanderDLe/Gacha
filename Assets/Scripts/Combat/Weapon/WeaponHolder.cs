using UnityEngine;

namespace RPG.Combat
{
    public class WeaponHolder : MonoBehaviour
    {
        public GameObject holdWeapon_GO;
        public float hitboxDebugRadius;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(holdWeapon_GO.transform.position, hitboxDebugRadius);
        }
    }
}