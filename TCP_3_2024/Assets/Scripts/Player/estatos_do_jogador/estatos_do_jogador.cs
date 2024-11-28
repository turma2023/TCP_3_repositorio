using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class estatos_do_jogador : MonoBehaviourPunCallbacks
{
    public int vida_inicial;
    public int vida_corente;

    public int escudo_inicial;
    public int escudo_corente;

    public int dinheiro_inicial;
    public int dinheiro_corente;
    public Text texto_do_dinheiro;

    // LayerMask usado para identificar os pontos de respawn
    public LayerMask layerDeRespawn;

    // Lista de pontos de respawn
    public Transform[] pontosDeRespawn;

    // Referência para o objeto jogador
    public Transform jogadorTransform;

    void Start()
    {
        vida_corente = vida_inicial;
        escudo_corente = escudo_inicial;
        dinheiro_corente = dinheiro_inicial;

        // Busca todos os objetos na cena e filtra os que pertencem à layer
        GameObject[] objetosDeRespawn = GameObject.FindObjectsOfType<GameObject>();
        List<Transform> pontosEncontrados = new List<Transform>();

        foreach (GameObject obj in objetosDeRespawn)
        {
            // Verifica se o objeto está na layer do LayerMask
            if (((1 << obj.layer) & layerDeRespawn) != 0)
            {
                pontosEncontrados.Add(obj.transform);
            }
        }

        // Converte para um array de Transform
        pontosDeRespawn = pontosEncontrados.ToArray();

        // Se não houver pontos de respawn definidos, gerar um erro
        if (pontosDeRespawn.Length == 0)
        {
            Debug.LogError("Nenhum ponto de respawn encontrado na LayerMask especificada!");
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            texto_do_dinheiro.text = dinheiro_corente.ToString();

            // Verifica se a vida chegou a zero e faz o respawn
            if (vida_corente <= 0)
            {
                photonView.RPC("Morrer", RpcTarget.AllBuffered);
            }
        }
    }

    public void tomar_dano(int dano)
    {
        if (!photonView.IsMine) return;

        // Primeiro, o dano é aplicado ao escudo
        if (escudo_corente > 0)
        {
            int danoRestante = dano - escudo_corente;
            escudo_corente = Mathf.Max(0, escudo_corente - dano);

            // Se sobrar dano após consumir o escudo, ele é aplicado à vida
            if (danoRestante > 0)
            {
                vida_corente = Mathf.Max(0, vida_corente - danoRestante);
            }
        }
        else
        {
            // Se não houver escudo, o dano é aplicado diretamente à vida
            vida_corente = Mathf.Max(0, vida_corente - dano);
        }
    }

    public void descontar_dinheiro(int custo)
    {
        if (!photonView.IsMine) return;
        dinheiro_corente -= custo;
    }

    [PunRPC]
    public void Morrer()
    {
        if (photonView.IsMine)
        {
            RespawnJogador();
        }
    }

    // Função para respawn do jogador
    void RespawnJogador()
    {
        if (!photonView.IsMine) return;

        // Se houver pontos de respawn definidos, o jogador será teleportado para um deles
        if (pontosDeRespawn.Length > 0)
        {
            // Seleciona aleatoriamente um ponto de respawn
            int pontoAleatorio = Random.Range(0, pontosDeRespawn.Length);
            jogadorTransform.position = pontosDeRespawn[pontoAleatorio].position;

            // Restaura a vida e o escudo do jogador
            vida_corente = vida_inicial;
            escudo_corente = escudo_inicial;

            // Sincroniza a restauração com todos os jogadores
            photonView.RPC("SincronizarRespawn", RpcTarget.AllBuffered, jogadorTransform.position);
        }
    }

    [PunRPC]
    public void SincronizarRespawn(Vector3 novaPosicao)
    {
        // Atualiza a posição do jogador em todas as máquinas
        jogadorTransform.position = novaPosicao;
    }
}
