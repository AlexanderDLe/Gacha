using UnityEngine;

public class DeactivateAfterSeconds : MonoBehaviour
{
    float currentTimer = 0;
    float maxLifetime = 5;
    bool runTimer = false;

    private void Update()
    {
        if (!runTimer) return;

        currentTimer += Time.deltaTime;
        if (currentTimer > maxLifetime)
        {
            runTimer = false;
            gameObject.SetActive(false);
        }
    }

    public void Initialize(float maxLifetime)
    {
        currentTimer = 0;
        this.maxLifetime = maxLifetime;
        runTimer = true;
    }
}
