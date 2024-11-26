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
    private int muni�ao_atual;
    private int muni�ao_guardada;


    public Text vida_UI;
    public Text escudo_UI;
    public Text muni�ao_UI;
    public Text muni�ao_guardada_UI;



    // Update is called once per frame
    void Update()
    {
        vida = vida_e_escudo.vida_corente;
        escudo = vida_e_escudo.escudo_corente;

        muni�ao_atual = arma.muni�ao_do_caregador;
        muni�ao_guardada = arma.muni�ao_guradada;

        // Atualiza os textos na interface
        vida_UI.text = vida.ToString();  
        escudo_UI.text = escudo.ToString();  
        muni�ao_UI.text = muni�ao_atual.ToString();  
        muni�ao_guardada_UI.text = muni�ao_guardada.ToString();

    }
}
