using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estatos_Do_Jogador : MonoBehaviour
{
    public int vida_do_jogador;
    public int vida_corente_do_jogador;

    public int escudo_do_jogador;
    public int escudo_corente_do_jogador;

    public int dinheiro_do_jogador;
    public int dinheiro_corente_do_jogador;




    void Start()
    {

        dinheiro_corente_do_jogador = dinheiro_do_jogador;
        escudo_corente_do_jogador = escudo_do_jogador;
        vida_corente_do_jogador = vida_do_jogador;
    }


    private void Update()
    {
        if(vida_corente_do_jogador <= 0) 
        {
            morrer();
        }
    }

    public void receber_dano( int dano)
    {
        // Primeiro, aplicar dano no escudo
        if (escudo_corente_do_jogador > 0)
        {
            int dano_restante = dano - escudo_corente_do_jogador; // Calcula se o dano ultrapassa o escudo
            escudo_corente_do_jogador -= dano; // Aplica o dano no escudo
            escudo_corente_do_jogador = Mathf.Max(escudo_corente_do_jogador, 0); // Garante que não fique negativo

            // Se sobrar dano, aplicar na vida
            if (dano_restante > 0)
            {
                vida_corente_do_jogador -= dano_restante;
                vida_corente_do_jogador = Mathf.Max(vida_corente_do_jogador, 0); // Garante que não fique negativo
            }
        }
        else
        {
            // Caso não tenha escudo, todo o dano vai para a vida
            vida_corente_do_jogador -= dano;
            vida_corente_do_jogador = Mathf.Max(vida_corente_do_jogador, 0); // Garante que não fique negativo
        }
    }
    public void morrer()
    {



    }
    public void remover_dinehiro( int custo)
    {
        Debug.LogError("cobrado");
        dinheiro_corente_do_jogador -= custo;
        dinheiro_corente_do_jogador = Mathf.Max(dinheiro_corente_do_jogador, 0); // Garante que não fique negativo
    }

}
