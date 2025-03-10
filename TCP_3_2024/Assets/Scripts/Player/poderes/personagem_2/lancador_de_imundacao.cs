using UnityEngine;
using Fusion;

public class lancador_de_imundacao : Skill
{
    public GameObject imundacao;




    // Update is called once per frame
    void Update()
    {
        if (!HasUsed && Input.GetKeyDown(KeyCode.X) && Object.HasInputAuthority)
        {
            RPC_imundacao();
            DisableUse();
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_imundacao()
    {
        if (Runner.IsServer)
        {
            if (imundacao != null)
            {
                Vector3 spawnPosition = transform.position;
                Quaternion spawnRotation = transform.rotation;

                NetworkObject shockwaveNetworkObject = Runner.Spawn(imundacao, spawnPosition, spawnRotation, Object.InputAuthority);

              
                shockwaveNetworkObject.gameObject.tag = gameObject.tag;
            }
        }
    }

}
