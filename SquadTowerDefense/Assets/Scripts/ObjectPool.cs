using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance; // Singleton instance
    public Transform spawnPoint;

    [System.Serializable]
    public class ObjectPoolItem
    {
        public GameObject prefab;
        public int poolSize;
    }

    public List<ObjectPoolItem> objectPoolItems; // List of object types and their pool sizes

    private Dictionary<GameObject, List<GameObject>> objectPools; // Dictionary to hold different pools

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of the object pool exists
        if (Instance == null)
        {
            Instance = this;
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        objectPools = new Dictionary<GameObject, List<GameObject>>();

        // Instantiate and populate the object pools with objects
        foreach (ObjectPoolItem item in objectPoolItems)
        {
            List<GameObject> pool = new List<GameObject>();

            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject obj = Instantiate(item.prefab, spawnPoint.position, quaternion.identity);
                obj.SetActive(false);
                pool.Add(obj);
            }

            objectPools.Add(item.prefab, pool);
        }
    }

    // Retrieve an object from the pool
    public GameObject GetObject(GameObject prefab)
    {
        if (objectPools.ContainsKey(prefab))
        {
            // Find the first inactive object in the pool
            foreach (GameObject obj in objectPools[prefab])
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }

            // If no inactive objects are found, instantiate a new one (expand the pool)
            GameObject newObj = Instantiate(prefab, spawnPoint.position, quaternion.identity);
            objectPools[prefab].Add(newObj);
            return newObj;
        }
        else
        {
            Debug.LogError("Prefab not found in object pool.");
            return null;
        }
    }

    // Return an object to the pool
    public void ReturnObjectToPool(GameObject prefab, GameObject obj)
    {
        if (objectPools.ContainsKey(prefab))
        {
            obj.SetActive(false);
        }
        else
        {
            Debug.LogError("Prefab not found in object pool.");
        }
    }
}
