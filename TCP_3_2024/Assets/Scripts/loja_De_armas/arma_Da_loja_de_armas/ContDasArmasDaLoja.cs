using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContDasArmasDaLoja : MonoBehaviour, IPointerClickHandler
{
    public Estatos_Do_Jogador estatosdejogador;
    public Controlador_de_armas controladordearma;
    private int dinheiro_do_jogador;

    public ScriptableObject_de_armas scriptableObjectDeArmas;
    RawImage lojaImageUI;
    string arma_nome;
    int custo_da_arma;

    
    void Start()
    {
        custo_da_arma = scriptableObjectDeArmas.valor_de_compra;
        dinheiro_do_jogador = estatosdejogador.dinheiro_corente_do_jogador;

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
        if (dinheiro_do_jogador >= custo_da_arma)
        {
           controladordearma.CromprarArma(scriptableObjectDeArmas);  // Compra a arma
            estatosdejogador.remover_dinehiro(custo_da_arma);  // Subtrai o dinheiro do jogador
            Debug.Log("Voc� comprou a arma!");
        }
        else
        {
            Debug.Log("Dinheiro insuficiente para comprar a arma.");
        }
    }
}
