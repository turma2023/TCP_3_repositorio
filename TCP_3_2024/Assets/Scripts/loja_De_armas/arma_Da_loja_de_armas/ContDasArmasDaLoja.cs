using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContDasArmasDaLoja : MonoBehaviour, IPointerClickHandler
{

   
    private int dinheiro_do_jogador;

    public ScriptableObject_de_armas scriptableObjectDeArmas;
    RawImage lojaImageUI;
    string arma_nome;
    int custo_da_arma;

    


    // Start is called before the first frame update
    void Start()
    {
        custo_da_arma = scriptableObjectDeArmas.valor_de_compra;
    

      
        lojaImageUI = GetComponent<RawImage>();
        // Define a imagem da loja com a textura do ScriptableObject
        if (scriptableObjectDeArmas != null && lojaImageUI != null)
        {
            arma_nome = scriptableObjectDeArmas.nome_da_arma.ToString();
            lojaImageUI.texture = scriptableObjectDeArmas.imagem_de_arma_para_a_loja;
        }
        else
        {
            Debug.LogWarning("ScriptableObject ou RawImage não estão definidos.");
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
      

     
      
    }



}
