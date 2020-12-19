using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int size;
    }

    [InfoBox("Prefab names must be unique. Objects in the pool are retrieved by name so names cannot be identical.")]
    public GameObject debugObject;
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        BuildAssets();
        BuildPools();
    }

    public void Initialize()
    {
        BuildAssets();
    }

    private void BuildAssets()
    {
        if (pools == null)
        {
            pools = new List<Pool>();
        }
        if (poolDictionary == null)
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
        }
    }

    private void BuildPools()
    {
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            CheckEmptyPool(pool);

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(gameObject.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.prefab.name, objectPool);
        }
    }

    private void CheckEmptyPool(Pool pool)
    {
        if (pool.size == 0)
        {
            print("Pool size cannot be set to 0. Size for " + pool.prefab.name + " set to 3.");
            pool.size = 3;
        }
    }

    public void AddToPool(GameObject prefab, int size)
    {
        if (poolDictionary.ContainsKey(prefab.name)) return;

        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(gameObject.transform);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        poolDictionary.Add(prefab.name, objectPool);
    }

    public void RemoveFromPool(string prefabName)
    {
        if (poolDictionary.ContainsKey(prefabName))
        {
            poolDictionary.Remove(prefabName);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Projectile not found in pool.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}

