using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class lancar_granada : NetworkBehaviour
{
    public GameObject grenadePrefab; // Prefab da granada na rede
    public float throwForce = 10f; // Forca do lançamento
    public Camera playerCamera; // Referencia à camera do jogador

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && Object.HasInputAuthority)
        {
            RPC_ThrowGrenade();
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_ThrowGrenade()
    {
        if (Runner.IsServer)
        {
            if (grenadePrefab != null)
            {
                Vector3 spawnPosition = playerCamera.transform.position + playerCamera.transform.forward * 2f;
                Quaternion spawnRotation = playerCamera.transform.rotation;

                // Spawna a granada na rede
                NetworkObject grenadeNetworkObject = Runner.Spawn(grenadePrefab, spawnPosition, spawnRotation, Object.InputAuthority);

                // Ajusta a tag da granada para corresponder ao jogador que a criou
                grenadeNetworkObject.gameObject.tag = gameObject.tag;

                // Aplica a força para lançar a granada
                if (grenadeNetworkObject.TryGetComponent(out Rigidbody rb))
                {
                    rb.velocity = playerCamera.transform.forward * throwForce;
                }
            }
        }
    }
}
