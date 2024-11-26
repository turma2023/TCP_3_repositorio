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





    void Start()
    {


        vida_corente = vida_inicial;
        escudo_corente = escudo_inicial;
        dinheiro_corente = dinheiro_inicial;
    }
    void Update()
    {
        texto_do_dinheiro.text = dinheiro_corente.ToString();
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





}