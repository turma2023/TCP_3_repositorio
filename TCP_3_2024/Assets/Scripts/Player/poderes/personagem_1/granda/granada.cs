using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class granada : NetworkBehaviour
{
    public float delay = 3f; // Tempo até a explosão
    public GameObject explosionEffect; // Prefab do efeito de explosão

    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            StartCoroutine(ExplodeAfterDelay());
        }
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = transform.rotation;
        RPC_Explode(spawnPosition, spawnRotation);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_Explode(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        if (Runner.IsServer)
        {
            if (explosionEffect != null)
            {
                
                NetworkObject explosionNetworkObject = Runner.Spawn(explosionEffect, spawnPosition, spawnRotation, Object.InputAuthority);

              
                explosionNetworkObject.gameObject.tag = gameObject.tag;
            }

           
            Runner.Despawn(Object);
        }
    }
}
