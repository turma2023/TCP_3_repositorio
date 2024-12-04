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
    public Text muniçao;
    public Text muniçao_guradada;


    void Start()
    {
        arma_equipada = armas.armaAtual;
    }

    void Update()
    {


        vida.text = Jogador.vida_corente_do_jogador.ToString();
        escudo.text = Jogador.escudo_corente_do_jogador.ToString();



        arma_equipada = armas.armaAtual; // Obtém a arma atual do controlador de armas

        // Atribui a quantidade de munição ao componente Text como uma string
        muniçao.text = arma_equipada.muniçao_do_caregador_da_arma.ToString();
        muniçao_guradada.text = arma_equipada.muniçao_guardada.ToString();
    }

}
