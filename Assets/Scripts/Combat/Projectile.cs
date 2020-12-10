using RPG.Attributes;
using RPG.Control;
using UnityEngine;
using RPG.AI;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        Vector3 projectileHeightAdjustment = new Vector3(0, .6f, 0);
        Vector3 destination = Vector3.zero;
        string layerToHarm;
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
        public void Initialize(Vector3 spawnPos, Vector3 destination, float speed, float damage, float projectileLifetime, string layerToHarm)
        {
            InitProj(spawnPos, destination, speed, damage, projectileLifetime, layerToHarm);
        }

        // Initialize with active lifetime (hitbox will become inactive before proj is disabled)
        public void Initialize(Vector3 spawnPos, Vector3 destination, float speed, float damage, float projectileLifetime, string layerToHarm, bool hasActiveLifetime, float activeLifetime)
        {
            InitProj(spawnPos, destination, speed, damage, projectileLifetime, layerToHarm);

            this.hasActiveLifetime = hasActiveLifetime;
            this.activeLifetime = activeLifetime;
        }

        private void InitProj(Vector3 spawnPos, Vector3 destination, float speed, float damage, float projectileLifetime, string layerToHarm)
        {
            this.destination = destination;
            transform.position = spawnPos;
            transform.LookAt(destination + projectileHeightAdjustment);

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
            if (other.gameObject.layer == LayerMask.NameToLayer(layerToHarm))
            {
                if (layerToHarm == "Player")
                {
                    BaseStats target = null;
                    target = other.gameObject.GetComponent<StateManager>().baseStats;
                    target.TakeDamage(damage);
                }
                else if (layerToHarm == "Enemy")
                {
                    AIManager target = null;
                    target = other.gameObject.GetComponent<AIManager>();
                    target.TakeDamage((int)damage);
                }
            }
        }

        private bool HitboxIsActive()
        {
            if (!hasActiveLifetime) return false;
            else return currentLifetime < activeLifetime;
        }
    }
}