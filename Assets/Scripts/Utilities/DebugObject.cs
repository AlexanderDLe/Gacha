using UnityEngine;

namespace RPG.Utility
{
    public class DebugObject : MonoBehaviour
    {
        public float currentLifetime = 0f;
        public float maxLifetime = 10f;

        private void Update()
        {
            currentLifetime += Time.deltaTime;
            if (currentLifetime > maxLifetime)
            {
                Reset();
                gameObject.SetActive(false);
            }
        }

        public void Initialize(Vector3 spawnPos, float scale, float maxLifetime)
        {
            gameObject.transform.position = spawnPos;
            gameObject.transform.localScale *= scale;
            this.maxLifetime = maxLifetime;
        }

        private void Reset()
        {
            currentLifetime = 0;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}