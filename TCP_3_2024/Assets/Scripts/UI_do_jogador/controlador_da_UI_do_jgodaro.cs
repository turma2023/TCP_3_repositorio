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
    public Text municao;
    public Text municao_guradada;


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
        municao.text = arma_equipada.municao_do_caregador_da_arma.ToString();
        municao_guradada.text = arma_equipada.municao_guardada.ToString();
    }

}
