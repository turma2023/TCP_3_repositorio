using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hud_do_jogador : MonoBehaviour
{
    public tiro_da_arma arma;
   public estatos_do_jogador vida_e_escudo;

    private int vida;
    private int escudo;
    private int muniçao_atual;
    private int muniçao_guardada;


    public Text vida_UI;
    public Text escudo_UI;
    public Text muniçao_UI;
    public Text muniçao_guardada_UI;



    // Update is called once per frame
    void Update()
    {
        vida = vida_e_escudo.vida_corente;
        escudo = vida_e_escudo.escudo_corente;

        muniçao_atual = arma.muniçao_do_caregador;
        muniçao_guardada = arma.muniçao_guradada;

        // Atualiza os textos na interface
        vida_UI.text = vida.ToString();  
        escudo_UI.text = escudo.ToString();  
        muniçao_UI.text = muniçao_atual.ToString();  
        muniçao_guardada_UI.text = muniçao_guardada.ToString();

    }
}
