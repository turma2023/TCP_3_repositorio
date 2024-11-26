using System.Diagnostics.SymbolStore;
using UnityEngine;

public class GerenciadorDeTrocaDeArmas : MonoBehaviour
{
    public ScriptableObject_de_armas arma1;
    public ScriptableObject_de_armas arma2;
    public ScriptableObject_de_armas arma3;

    public GameObject armaAtual;
    public int dano_de_arma;
    public int muniçao_do_caregador;
    public int muniçao_guradada;
    public float tiros_por_minuto;
    public float renge_de_diparos;

    public Transform pontoDeEquipamento;

    private int muniçao_guardada_temp; // Variável para guardar temporariamente a munição guardada

    void Start()
    {
        if (pontoDeEquipamento == null)
        {
            Debug.LogError("Ponto de equipamento não foi definido! A arma não será instanciada corretamente.");
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

        // Salva a munição guardada antes de trocar de arma
        muniçao_guardada_temp = muniçao_guradada;

        // Instancia a nova arma no ponto de equipamento
        armaAtual = Instantiate(arma.modelo_da_arma, pontoDeEquipamento.position, pontoDeEquipamento.rotation);
        armaAtual.transform.SetParent(pontoDeEquipamento);
        armaAtual.transform.localPosition = Vector3.zero;
        armaAtual.transform.localRotation = Quaternion.identity;

        // Atualiza as estatísticas da nova arma
        dano_de_arma = arma.dano_da_arma;
        muniçao_do_caregador = arma.muniçao_do_caregador_da_arma;
        muniçao_guradada = muniçao_guardada_temp;  // Retorna a munição guardada ao trocar de arma
        tiros_por_minuto = arma.cadencia_de_disparos;
        renge_de_diparos = arma.distancia_deo_RaycastHit;
        muniçao_guradada = arma.muniçao_guardada;
    }

    public void AtualizarMunicao(int municaoAtualCarregador, int municaoGuardada)
    {
        muniçao_do_caregador = municaoAtualCarregador;
        muniçao_guradada = municaoGuardada;
        Debug.Log($"Munição atualizada: Carregador = {muniçao_do_caregador}, Guardada = {muniçao_guradada}");
    }

    public void RecebeArma(ScriptableObject_de_armas novaArma)
    {
        // Verifica se há um slot disponível e adiciona a arma
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
            Debug.Log("O inventário de armas está cheio. Não é possível adicionar novas armas.");
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
