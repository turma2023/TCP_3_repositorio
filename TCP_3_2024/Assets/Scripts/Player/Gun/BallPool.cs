using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class BallPool : NetworkBehaviour
{
    public NetworkObject prefab;
    public int poolSize = 10;
    private List<NetworkObject> pool;

    void Start()
    {
        pool = new List<NetworkObject>();

        for (int i = 0; i < poolSize; i++)
        {
            NetworkObject obj = Runner.Spawn(prefab, Vector3.zero, Quaternion.identity, null, (runner, spawnedObj) =>
            {
                spawnedObj.gameObject.SetActive(false);
            });
            pool.Add(obj);
        }
    }

    public NetworkObject GetObject()
    {
        foreach (NetworkObject obj in pool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        NetworkObject newObj = Runner.Spawn(prefab, Vector3.zero, Quaternion.identity, null, (runner, spawnedObj) =>
        {
            spawnedObj.gameObject.SetActive(true);
        });
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnObject(NetworkObject obj)
    {
        obj.gameObject.SetActive(false);
    }
}
