using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeCharacter : NetworkBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    private Button btnPlayer;
    public NetworkObject prefabPlayer;

    private Spawner spawner;
    private NetworkObject spawnedPlayer;
    private NetworkRunner runner;

    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        runner = spawner.GetComponent<NetworkRunner>();
        btnPlayer = GetComponent<Button>();
        // btnPlayer.onClick.AddListener(() => spawner.SetSelectedCharacter(prefabPlayer));
    }

        // Método chamado quando o mouse entra no botão
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entrou no botão");
        SpawnPlayerAtCenter();
    }

    // Método chamado quando o mouse sai do botão
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse saiu do botão");
        DespawnPlayer();
    }

    void SpawnPlayerAtCenter()
    {
        if (spawnedPlayer == null)
        {
            if(Object.HasStateAuthority){
                Vector3 centerPosition = new Vector3(Screen.width / 2, Screen.height / 2, 10);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(centerPosition);

                Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward);
                rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y + 180, rotation.eulerAngles.z);

                // spawnedPlayer = runner.Spawn(prefabPlayer, worldPosition, rotation);
                spawnedPlayer = Instantiate(prefabPlayer, worldPosition, rotation);


                spawnedPlayer.GetComponentInChildren<Camera>().transform.rotation = rotation;
            }
        }
    }

    void DespawnPlayer()
    {
        if (spawnedPlayer != null)
        {
            // runner.Despawn(spawnedPlayer);
            Debug.LogError("foi chamado");
            Destroy(spawnedPlayer);
            spawnedPlayer = null;
        }
    }



}
