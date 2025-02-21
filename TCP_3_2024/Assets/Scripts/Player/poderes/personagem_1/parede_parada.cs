using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ParedeParada : Skill
{
    public GameObject wallPreviewPrefab; // Prefab do preview da parede
    public GameObject wallRealPrefab; 
    public KeyCode placeKey = KeyCode.Q; // Tecla para instanciar a parede
    public KeyCode rotateKey = KeyCode.R; // Tecla para rotacionar o preview
    public LayerMask groundLayer; // Camada do chao para posicionar a parede

    private GameObject currentPreview; // Preview atual
    private bool isPreviewActive = false; // Indica se o preview est� ativo

    private bool activeRotation;

    void Update()
    {
        if (Input.GetKeyDown(placeKey) && Object.HasInputAuthority && !HasUsed)
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
                DisableUse();
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

        Vector3 shootDirection = GetComponent<PlayerController>().camera.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(GetComponent<PlayerController>().camera.transform.position, shootDirection, out hit, 10, groundLayer))
        {
            currentPreview.transform.position = hit.point;
            currentPreview.transform.rotation = GetComponent<PlayerController>().transform.rotation;
            Debug.LogError(hit.point);
            if (activeRotation)
            { 
                currentPreview.transform.Rotate(new Vector3(0, 90, 0), Space.Self);
            }
        }
    }

    void RotatePreview()
    {
        activeRotation = !activeRotation;
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
