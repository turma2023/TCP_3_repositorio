using UnityEngine;
using Fusion;
public class SpawnSmoke : Skill
{
    private NetworkRunner runner;
    public NetworkObject smoke;
    public NetworkObject previewPrefab;
    public float range = 10f;
    private NetworkObject previewInstance;
    private bool onPreview;

    private void Start()
    {
        if (Object.HasInputAuthority)
            //IconSmoke.gameObject.SetActive(true);

        runner = FindObjectOfType<Spawner>().Runner;
        smoke.GetComponent<ScaleSmoke>().runner = runner;
        //IconSmokePadrao = IconSmoke.color;

    }

    private void Update()
    {
        if (!HasUsed)
        {
            if (Object.HasInputAuthority)
            {
                if (Input.GetKeyDown(KeyCode.Q) && !onPreview)
                {
                    onPreview = true;
                    return;
                }

                if (onPreview)
                {
                    Vector3 targetPosition = GetTargetPosition();
                    if (previewInstance == null)
                    {
                        previewInstance = Instantiate(previewPrefab, targetPosition, Quaternion.identity);
                    }
                    else
                    {
                        previewInstance.transform.position = targetPosition;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Q) && onPreview)
                {
                    if (previewInstance != null)
                    {
                        RPC_SpawnSmoke(previewInstance.transform.position);
                        Destroy(previewInstance.gameObject);
                        DisableUse();
                    }
                    onPreview = false;
                }
            }
        }
        else
        {
        }
    }

    public void ActivateAgain()
    {
        EnableUse();
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
