using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class aparecimento_do_jogador : MonoBehaviour
{
    public Transform[] spawnPoints;   // Array de pontos de spawn no mapa (arraste no Inspector)
    public GameObject playerPrefab;   // Prefab do jogador (arraste o prefab do jogador no Inspector)

    void Start()
    {
        // Instancia o jogador quando ele entra na sala
        SpawnPlayer();
    }

    // Função para fazer o jogador aparecer aleatoriamente em um ponto de spawn
    void SpawnPlayer()
    {
        // Verifica se há pontos de spawn
        if (spawnPoints.Length > 0)
        {
            // Escolhe um ponto de spawn aleatório
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];

            // Instancia o jogador na posição aleatória escolhida
            // Usamos PhotonNetwork.Instantiate para garantir que a instanciação seja sincronizada
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Nenhum ponto de spawn encontrado!");
        }
    }
}
