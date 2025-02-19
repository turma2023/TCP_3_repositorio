using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ParedeParada : NetworkBehaviour
{
    public GameObject wallPreviewPrefab; // Prefab do preview da parede
    public GameObject wallRealPrefab; 
    public KeyCode placeKey = KeyCode.Q; // Tecla para instanciar a parede
    public KeyCode rotateKey = KeyCode.R; // Tecla para rotacionar o preview
    public LayerMask groundLayer; // Camada do chao para posicionar a parede

    private GameObject currentPreview; // Preview atual
    private bool isPreviewActive = false; // Indica se o preview está ativo

    void Update()
    {
        if (Input.GetKeyDown(placeKey) && Object.HasInputAuthority)
        {
            if (!isPreviewActive)
            {
                StartPreview();
            }
            else
            {
                Vector3 spawnPosition = currentPreview.transform.position;
                Quaternion spawnRotation = currentPreview.transform.rotation;

                // Destroi o preview local
                Destroy(currentPreview);
                isPreviewActive = false;

                // Chama o RPC para spawnar a parede real
                RPC_PlaceWall(spawnPosition, spawnRotation);
            }
        }

        if (isPreviewActive && Input.GetKeyDown(rotateKey))
        {
            RotatePreview();
        }

        if (isPreviewActive)
        {
            UpdatePreviewPosition();
        }
    }

    void StartPreview()
    {
        currentPreview = Instantiate(wallPreviewPrefab);
        isPreviewActive = true;
    }

    void UpdatePreviewPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            currentPreview.transform.position = hit.point;
        }
    }

    void RotatePreview()
    {
        currentPreview.transform.Rotate(0, 90, 0);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_PlaceWall(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        if (wallRealPrefab != null)
        {
          
            NetworkObject wallNetworkObject = Runner.Spawn(wallRealPrefab, spawnPosition, spawnRotation, Object.InputAuthority);

            
            wallNetworkObject.gameObject.tag = gameObject.tag;
        }
    }
}
