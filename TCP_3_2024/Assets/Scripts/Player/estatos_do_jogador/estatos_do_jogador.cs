using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class estatos_do_jogador : MonoBehaviour
{
    public int vida_inicial;
    public int vida_corente;

    public int escudo_inicial;
    public int escudo_corente;

    public int dinheiro_inicial;
    public int dinheiro_corente;
    public Text texto_do_dinheiro;

    // Lista de pontos de respawn
    public Transform[] pontosDeRespawn;

    // Refer�ncia para o objeto jogador
    public Transform jogadorTransform;

    void Start()
    {
        vida_corente = vida_inicial;
        escudo_corente = escudo_inicial;
        dinheiro_corente = dinheiro_inicial;

        // Se n�o houver pontos de respawn definidos, gerar um erro
        if (pontosDeRespawn.Length == 0)
        {
            Debug.LogError("Nenhum ponto de respawn definido!");
        }
    }

    void Update()
    {
        texto_do_dinheiro.text = dinheiro_corente.ToString();

        // Verifica se a vida chegou a zero e faz o respawn
        if (vida_corente <= 0)
        {
            RespawnJogador();
        }
    }

    public void tomar_dano(int dano)
    {
        // Primeiro, o dano � aplicado ao escudo
        if (escudo_corente > 0)
        {
            int danoRestante = dano - escudo_corente;
            escudo_corente = Mathf.Max(0, escudo_corente - dano);

            // Se sobrar dano ap�s consumir o escudo, ele � aplicado � vida
            if (danoRestante > 0)
            {
                vida_corente = Mathf.Max(0, vida_corente - danoRestante);
            }
        }
        else
        {
            // Se n�o houver escudo, o dano � aplicado diretamente � vida
            vida_corente = Mathf.Max(0, vida_corente - dano);
        }
    }

    public void descontar_dinheiro(int custo)
    {
        dinheiro_corente -= custo;
    }

    // Fun��o para respawn do jogador
    void RespawnJogador()
    {
        // Se houver pontos de respawn definidos, o jogador ser� teleportado para um deles
        if (pontosDeRespawn.Length > 0)
        {
            // Seleciona aleatoriamente um ponto de respawn
            int pontoAleatorio = Random.Range(0, pontosDeRespawn.Length);
            jogadorTransform.position = pontosDeRespawn[pontoAleatorio].position;

            // Restaura a vida e o escudo do jogador
            vida_corente = vida_inicial;
            escudo_corente = escudo_inicial;
        }
    }
}
