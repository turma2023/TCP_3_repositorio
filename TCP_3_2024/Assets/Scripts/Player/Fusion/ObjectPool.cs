using System.Collections.Generic;
using UnityEngine;
using Fusion;

public static class ObjectPool
{
    private static Dictionary<NetworkObject, Queue<NetworkObject>> pool = new Dictionary<NetworkObject, Queue<NetworkObject>>();

    public static NetworkObject Get(NetworkObject prefab)
    {
        if (!pool.ContainsKey(prefab))
        {
            pool[prefab] = new Queue<NetworkObject>();
        }

        if (pool[prefab].Count > 0)
        {
            NetworkObject instance = pool[prefab].Dequeue();
            instance.gameObject.SetActive(true);
            return instance;
        }
        else
        {
            return Object.Instantiate(prefab);
        }
    }

    public static void Return(NetworkObject instance)
    {
        instance.gameObject.SetActive(false);
        if (!pool.ContainsKey(instance))
        {
            pool[instance] = new Queue<NetworkObject>();
        }

        pool[instance].Enqueue(instance);
    }
}
