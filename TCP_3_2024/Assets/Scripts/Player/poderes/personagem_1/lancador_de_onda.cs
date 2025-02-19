using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class LancadorDeOnda : NetworkBehaviour
{
    public GameObject shockwavePrefab; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Object.HasInputAuthority)
        {
            RPC_LaunchShockwave();
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_LaunchShockwave()
    {
        if (Runner.IsServer)
        {
            if (shockwavePrefab != null)
            {
                Vector3 spawnPosition = transform.position;
                Quaternion spawnRotation = transform.rotation;

           
                NetworkObject shockwaveNetworkObject = Runner.Spawn(shockwavePrefab, spawnPosition, spawnRotation, Object.InputAuthority);

                
                shockwaveNetworkObject.gameObject.tag = gameObject.tag;
            }
        }
    }
}
