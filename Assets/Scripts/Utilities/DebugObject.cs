using UnityEngine;

namespace RPG.Utility
{
    public class DebugObject : MonoBehaviour
    {
        public float currentLifetime = 0f;
        public float maxLifetime = 10f;

        public bool isMoving = false;
        public float distance = 5f;

        private void Update()
        {
            currentLifetime += Time.deltaTime;
            if (currentLifetime > maxLifetime)
            {
                Reset();
                gameObject.SetActive(false);
            }
            if (isMoving)
            {
                transform.position += transform.forward * Time.deltaTime * distance / maxLifetime;
            }
        }

        public void Initialize(Vector3 spawnPosition, Quaternion spawnRotation, float scale, float maxLifetime)
        {
            gameObject.transform.position = spawnPosition;
            gameObject.transform.rotation = spawnRotation;
            gameObject.transform.localScale *= scale;
            this.maxLifetime = maxLifetime;
        }

        private void Reset()
        {
            isMoving = false;
            currentLifetime = 0;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }

        public void MoveDistance(float distance)
        {
            isMoving = true;
            this.distance = distance;
        }
    }
}