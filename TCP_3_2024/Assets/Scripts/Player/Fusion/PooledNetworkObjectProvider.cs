using Fusion;
using UnityEngine;

public class PooledNetworkObjectProvider : NetworkObjectProviderDefault
{
    protected override NetworkObject InstantiatePrefab(NetworkRunner runner, NetworkObject prefab)
    {
        // Obtenha o objeto do pool
        NetworkObject instance = ObjectPool.Get(prefab);
        return instance;
    }

    protected override void DestroyPrefabInstance(NetworkRunner runner, NetworkPrefabId prefabId, NetworkObject instance)
    {
        // Retorne a instância ao pool
        ObjectPool.Return(instance);
    }
}
