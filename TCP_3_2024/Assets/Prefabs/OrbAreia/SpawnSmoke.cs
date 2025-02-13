using UnityEngine;
using Fusion;
using UnityEngine.UI;
public class SpawnSmoke : NetworkBehaviour 
{
    private NetworkRunner runner;
    public NetworkObject smoke;
    public NetworkObject previewPrefab;
    public float range = 10f;
    public Image IconSmoke;
    private Color IconSmokePadrao;
    private NetworkObject previewInstance;
    private bool used = false;


    private void Start()
    {
        if (Object.HasInputAuthority)
            IconSmoke.gameObject.SetActive(true);
        
        runner = FindObjectOfType<Spawner>().Runner;
        smoke.GetComponent<ScaleSmoke>().runner = runner;
        IconSmokePadrao = IconSmoke.color;

    }

    private void Update()
    {   
        if (!used)
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
                        used = true;
                        IconSmoke.color = Color.gray;
                    }
                }
            }
        }
        else{
            Debug.LogError("não pode usar novamente");
        }
    }

    public void ActivateAgain(){
        used = false;
        IconSmoke.color = IconSmokePadrao;
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
