using UnityEngine;
using Fusion;
public class SpawnSmoke : NetworkBehaviour 
{
    private NetworkRunner runner;
    public NetworkObject smoke;

    public NetworkObject previewPrefab;
    public float range = 10f;

    private NetworkObject previewInstance;


    private void Start()
    {
        runner = FindObjectOfType<Spawner>().Runner;
        smoke.GetComponent<ScaleSmoke>().runner = runner;
    }

    private void Update()
    {   
        if (Object.HasInputAuthority)
        {
            if (Input.GetKey(KeyCode.E))
            {
                Vector3 targetPosition = GetTargetPosition();

                if (previewInstance == null)
                {
                    previewInstance = Instantiate(previewPrefab, targetPosition, Quaternion.identity);
                }
                else{
                    previewInstance.transform.position = targetPosition;
                }
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                if (previewInstance != null)
                {
                    RPC_SpawnSmoke(previewInstance.transform.position);
                    Destroy(previewInstance.gameObject);
                }
            }
        }
    }



    private Vector3 GetTargetPosition()
    {
        Vector3 shootDirection = GetComponent<PlayerController>().camera.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(GetComponent<PlayerController>().camera.transform.position, shootDirection, out hit, range))
        {
            return hit.point;
        }
        
        return transform.position + transform.forward * range;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SpawnSmoke(Vector3 transform) 
    { 
        runner.Spawn(smoke, transform);
    }
}
