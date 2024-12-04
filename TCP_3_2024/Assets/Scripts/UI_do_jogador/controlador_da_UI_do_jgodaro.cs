using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlador_da_UI_do_jgodaro : MonoBehaviour
{
    public Controlador_de_armas armas;
    public  Estatos_Do_Jogador Jogador;

    public ScriptableObject_de_armas arma_equipada;

    public Text vida;
    public Text escudo;
    public Text muni�ao;
    public Text muni�ao_guradada;


    void Start()
    {
        arma_equipada = armas.armaAtual;
    }

    void Update()
    {


        vida.text = Jogador.vida_corente_do_jogador.ToString();
        escudo.text = Jogador.escudo_corente_do_jogador.ToString();



        arma_equipada = armas.armaAtual; // Obt�m a arma atual do controlador de armas

        // Atribui a quantidade de muni��o ao componente Text como uma string
        muni�ao.text = arma_equipada.muni�ao_do_caregador_da_arma.ToString();
        muni�ao_guradada.text = arma_equipada.muni�ao_guardada.ToString();
    }

}
