using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour
{
    [SerializeField] float timeToDestroy = 3f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToDestroy)
        {
            // print("Destroying: " + gameObject.name);
            Destroy(gameObject);
        }
    }
}
