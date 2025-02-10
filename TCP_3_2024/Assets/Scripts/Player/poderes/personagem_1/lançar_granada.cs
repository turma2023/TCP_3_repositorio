using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lançar_granada : MonoBehaviour
{
    public GameObject grenadePrefab; // Prefab da granada
    public float throwForce = 10f; // Força do lançamento
    public Camera playerCamera; // Referência à câmera do jogador

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // Tecla "G" para lançar a granada
        {
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {
        // Posição e rotação da câmera
        Vector3 spawnPosition = playerCamera.transform.position + playerCamera.transform.forward * 2f; // 2f é a distância na frente da câmera
        Quaternion spawnRotation = playerCamera.transform.rotation;

        // Instancia a granada na frente da câmera
        GameObject grenade = Instantiate(grenadePrefab, spawnPosition, spawnRotation);

        // Aplica força para lançar a granada
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.VelocityChange);
    }
}
