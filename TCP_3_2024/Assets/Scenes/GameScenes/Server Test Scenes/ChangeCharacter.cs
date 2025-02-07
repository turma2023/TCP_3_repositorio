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
    public NetworkObject playerPrefab;

    private Spawner spawner;
    private NetworkObject spawnedPlayer;
    private NetworkRunner runner;
    [SerializeField] private Image highlight;
    private bool hasClicked;

    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        runner = spawner.GetComponent<NetworkRunner>();
        btnPlayer = GetComponent<Button>();
        //highlight = GetComponentInChildren<Image>(true);
        // btnPlayer.onClick.AddListener(() => spawner.SetSelectedCharacter(prefabPlayer));
    }

        // Método chamado quando o mouse entra no botão
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hasClicked) return;
        Debug.Log("Mouse entrou no botão");
        highlight.gameObject.SetActive(true);
        //SpawnPlayerAtCenter();
    }

    // Método chamado quando o mouse sai do botão
    public void OnPointerExit(PointerEventData eventData)
    {
        if (hasClicked) return;
        Debug.Log("Mouse saiu do botão");
        highlight.gameObject.SetActive(false);
        //DespawnPlayer();
    }

    public void OnClick()
    {
        highlight.gameObject.SetActive(true);
        highlight.color = new Color(255 / 255f, 0 / 255f, 0 / 255f, 0.85f);
        hasClicked = !hasClicked;
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
                spawnedPlayer = Instantiate(playerPrefab, worldPosition, rotation);


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
