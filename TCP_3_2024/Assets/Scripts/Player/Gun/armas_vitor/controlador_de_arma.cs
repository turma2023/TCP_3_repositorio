using System.Diagnostics.SymbolStore;
using UnityEngine;

public class GerenciadorDeTrocaDeArmas : MonoBehaviour
{
    public ScriptableObject_de_armas arma1;
    public ScriptableObject_de_armas arma2;
    public ScriptableObject_de_armas arma3;

    public GameObject armaAtual;
    public int dano_de_arma;
    public int muni�ao_do_caregador;
    public int muni�ao_guradada;
    public float tiros_por_minuto;
    public float renge_de_diparos;

    public Transform pontoDeEquipamento;

    private int muni�ao_guardada_temp; // Vari�vel para guardar temporariamente a muni��o guardada

    void Start()
    {
        if (pontoDeEquipamento == null)
        {
            Debug.LogError("Ponto de equipamento n�o foi definido! A arma n�o ser� instanciada corretamente.");
            return;
        }

        EquiparArma(arma1); // Inicializa com a arma1
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TrocarArma(arma1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TrocarArma(arma2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TrocarArma(arma3);
        }
    }

    void TrocarArma(ScriptableObject_de_armas novaArma)
    {
        if (armaAtual != null)
        {
            Destroy(armaAtual);
        }

        EquiparArma(novaArma);
    }

    void EquiparArma(ScriptableObject_de_armas arma)
    {
        if (arma == null) return;

        // Salva a muni��o guardada antes de trocar de arma
        muni�ao_guardada_temp = muni�ao_guradada;

        // Instancia a nova arma no ponto de equipamento
        armaAtual = Instantiate(arma.modelo_da_arma, pontoDeEquipamento.position, pontoDeEquipamento.rotation);
        armaAtual.transform.SetParent(pontoDeEquipamento);
        armaAtual.transform.localPosition = Vector3.zero;
        armaAtual.transform.localRotation = Quaternion.identity;

        // Atualiza as estat�sticas da nova arma
        dano_de_arma = arma.dano_da_arma;
        muni�ao_do_caregador = arma.muni�ao_do_caregador_da_arma;
        muni�ao_guradada = muni�ao_guardada_temp;  // Retorna a muni��o guardada ao trocar de arma
        tiros_por_minuto = arma.cadencia_de_disparos;
        renge_de_diparos = arma.distancia_deo_RaycastHit;
        muni�ao_guradada = arma.muni�ao_guardada;
    }

    public void AtualizarMunicao(int municaoAtualCarregador, int municaoGuardada)
    {
        muni�ao_do_caregador = municaoAtualCarregador;
        muni�ao_guradada = municaoGuardada;
        Debug.Log($"Muni��o atualizada: Carregador = {muni�ao_do_caregador}, Guardada = {muni�ao_guradada}");
    }

    public void RecebeArma(ScriptableObject_de_armas novaArma)
    {
        // Verifica se h� um slot dispon�vel e adiciona a arma
        if (arma1 == null)
        {
            arma1 = novaArma;
            Debug.Log($"Arma {novaArma.nome_da_arma} adicionada ao slot 1.");
        }
        else if (arma2 == null)
        {
            arma2 = novaArma;
            Debug.Log($"Arma {novaArma.nome_da_arma} adicionada ao slot 2.");
        }
        else if (arma3 == null)
        {
            arma3 = novaArma;
            Debug.Log($"Arma {novaArma.nome_da_arma} adicionada ao slot 3.");
        }
        else
        {
            Debug.Log("O invent�rio de armas est� cheio. N�o � poss�vel adicionar novas armas.");
        }
    }

    public void vender_arma() 
    {
        arma1= null;
        arma2 = null;
        arma3 = null;
        Destroy(armaAtual);
    }
}
