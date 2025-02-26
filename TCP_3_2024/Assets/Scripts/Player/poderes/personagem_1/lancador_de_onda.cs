using UnityEngine;
using Fusion;

public class LancadorDeOnda : Skill
{
    public GameObject shockwavePrefab; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && Object.HasInputAuthority && !HasUsed)
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
                Vector3 spawnPosition = transform.position + transform.forward * 1.0f;;
                Quaternion spawnRotation = transform.rotation;

           
                NetworkObject shockwaveNetworkObject = Runner.Spawn(shockwavePrefab, spawnPosition, spawnRotation, Object.InputAuthority);

                
                shockwaveNetworkObject.gameObject.tag = gameObject.tag;
                DisableUse();
            }
        }
    }
}
