using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parede_parada : MonoBehaviour
{
    public GameObject wallPreviewPrefab; // Prefab do preview da parede
    public GameObject wallRealPrefab; // Prefab da parede real
    public KeyCode placeKey = KeyCode.Q; // Tecla para instanciar a parede
    public KeyCode rotateKey = KeyCode.R; // Tecla para rotacionar o preview
    public LayerMask groundLayer; // Camada do chão para posicionar a parede

    private GameObject currentPreview; // Preview atual
    private bool isPreviewActive = false; // Indica se o preview está ativo

    void Update()
    {
        // Ativa/desativa o preview ao pressionar Q
        if (Input.GetKeyDown(placeKey))
        {
            if (!isPreviewActive)
            {
                StartPreview();
            }
            else
            {
                PlaceWall();
            }
        }

        // Rotaciona o preview ao pressionar R
        if (isPreviewActive && Input.GetKeyDown(rotateKey))
        {
            RotatePreview();
        }

        // Atualiza a posição do preview com base no mouse
        if (isPreviewActive)
        {
            UpdatePreviewPosition();
        }
    }

    void StartPreview()
    {
        // Instancia o preview da parede
        currentPreview = Instantiate(wallPreviewPrefab);
        isPreviewActive = true;
    }

    void UpdatePreviewPosition()
    {
        // Posiciona o preview no local do mouse (raycast no chão)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            currentPreview.transform.position = hit.point;
        }
    }

    void RotatePreview()
    {
        // Rotaciona o preview em 90 graus
        currentPreview.transform.Rotate(0, 90, 0);
    }

    void PlaceWall()
    {
        // Instancia a parede real na posição do preview
        Instantiate(wallRealPrefab, currentPreview.transform.position, currentPreview.transform.rotation);

        // Destroi o preview e desativa o modo de preview
        Destroy(currentPreview);
        isPreviewActive = false;
    }
}
