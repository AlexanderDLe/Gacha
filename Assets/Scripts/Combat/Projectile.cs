using RPG.Attributes;
using RPG.Control;
using UnityEngine;
using RPG.AI;
using RPG.Core;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        Vector3 projectileHeightAdjustment = new Vector3(0, .6f, 0);
        Vector3 destination = Vector3.zero;
        LayerMask layerToHarm;
        float speed = 0f;
        float damage = 0f;
        float currentLifetime = 0f;
        float projectileLifetime = 5f;

        bool hasActiveLifetime = false;
        float activeLifetime = 5f;

        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            currentLifetime += Time.deltaTime;
            if (currentLifetime > projectileLifetime)
            {
                gameObject.SetActive(false);
            }
        }

        // Initialize without active lifetime (entire proj will be disabled altogether)
        public void Initialize(Vector3 spawnPos, Vector3 destination, float speed, float damage, float projectileLifetime, LayerMask layerToHarm)
        {
            InitProj(spawnPos, destination, speed, damage, projectileLifetime, layerToHarm);
        }

        // Initialize with active lifetime (hitbox will become inactive before proj is disabled)
        public void Initialize(Vector3 spawnPos, Vector3 destination, float speed, float damage, float projectileLifetime, LayerMask layerToHarm, bool hasActiveLifetime, float activeLifetime)
        {
            InitProj(spawnPos, destination, speed, damage, projectileLifetime, layerToHarm);

            this.hasActiveLifetime = hasActiveLifetime;
            this.activeLifetime = activeLifetime;
        }

        private void InitProj(Vector3 spawnPos, Vector3 destination, float speed, float damage, float projectileLifetime, LayerMask layerToHarm)
        {
            this.destination = destination;
            transform.position = spawnPos;

            // We want it to look at the same level as the spawn position so it doesn't angle upwards or downwards as it moves.
            transform.LookAt(new Vector3(destination.x, spawnPos.y, destination.z));

            this.speed = speed;
            this.damage = damage;

            this.currentLifetime = 0;
            this.projectileLifetime = projectileLifetime;

            this.layerToHarm = layerToHarm;
        }

        /*  Important note regarding Particle Projectiles
        
            Particle Projectiles may require more specialized attention than just ordinary projectiles. A particle projectile may have multiple Particle Systems that combine together. It may be comprised of a "Projectile" particle system along with a "Effects" particle system.

            In these cases, set the actual "projectile" particle system to LOCAL simulation space so that it follows the spawning hitbox and projectile configurations. 
            
            The "effect" particle systems (such as rocks, flames, trails, and sparks) may sometimes need to remain in WORLD simulation space so that it does not follow the local space. In this case, set Motion Vectors to "Force To Motion":
            
            "Particle System>Renderer>Motion Vectors"
         */
        private void OnTriggerEnter(Collider other)
        {
            if (!HitboxIsActive()) return;
            /*  layer.value returns a value to the second power (x ^ 2).
                Therefore, we must take the log of that value before comparison.*/
            if (other.gameObject.layer == Mathf.Log(layerToHarm.value, 2))
            {
                BaseManager target = null;
                target = other.gameObject.GetComponent<BaseManager>();
                target.TakeDamage((int)damage);
            }
        }

        private bool HitboxIsActive()
        {
            if (!hasActiveLifetime) return true;
            else return currentLifetime < activeLifetime;
        }
    }
}