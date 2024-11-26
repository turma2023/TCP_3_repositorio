using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContDasArmasDaLoja : MonoBehaviour, IPointerClickHandler
{

    public estatos_do_jogador Estatos_Do_Jogador;
    private int dinheiro_do_jogador;

    public ScriptableObject_de_armas scriptableObjectDeArmas; 
    RawImage lojaImageUI; 
    string arma_nome;
    int custo_da_arma;

    public GerenciadorDeTrocaDeArmas gerenciadorDeTrocaDeArmas;
    

    // Start is called before the first frame update
    void Start()
    {
        custo_da_arma = scriptableObjectDeArmas.valor_de_compra;
        dinheiro_do_jogador = Estatos_Do_Jogador.dinheiro_corente;

        gerenciadorDeTrocaDeArmas = FindObjectOfType<GerenciadorDeTrocaDeArmas>();
        lojaImageUI = GetComponent<RawImage>();
        // Define a imagem da loja com a textura do ScriptableObject
        if (scriptableObjectDeArmas != null && lojaImageUI != null)
        {
            arma_nome = scriptableObjectDeArmas.nome_da_arma.ToString();
            lojaImageUI.texture = scriptableObjectDeArmas.imagem_de_arma_para_a_loja;
        }
        else
        {
            Debug.LogWarning("ScriptableObject ou RawImage n�o est�o definidos.");
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clique detectado em {arma_nome}");

        if (gerenciadorDeTrocaDeArmas != null && custo_da_arma <= dinheiro_do_jogador)
        {
            gerenciadorDeTrocaDeArmas.RecebeArma(scriptableObjectDeArmas);
            Estatos_Do_Jogador.descontar_dinheiro(custo_da_arma);
        }
        else
        {
            Debug.LogWarning("Dinheiro insuficiente ou GerenciadorDeTrocaDeArmas n�o encontrado.");
        }
    }



}
