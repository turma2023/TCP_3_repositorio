using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiro_da_arma : MonoBehaviour
{
    public GerenciadorDeTrocaDeArmas gerenciadorDeTrocaDeArmas;
    private GameObject arma_atual;

    public GameObject cam;

    private int dano;
    public int muni�ao_do_caregador;
    public int muni�ao_guradada;
    public int muni�ao_corente; // A muni��o que est� sendo usada no carregador
    private float tirospor_minuto;
    private float tempoDeUltimoTiro = 0f;

    // Vari�veis para armazenar o estado da muni��o da arma atual
  

    void Start()
    {
        AtualizarEstatisticas();
        // Inicializa a muni��o da arma atual
    }

    void Update()
    {
        // Atualiza as estat�sticas se a arma atual mudar
        if (arma_atual != gerenciadorDeTrocaDeArmas.armaAtual)
        {
          
         

            // Atualiza a arma atual
            arma_atual = gerenciadorDeTrocaDeArmas.armaAtual;
            AtualizarEstatisticas();

            // Restaura a muni��o da arma anterior se houver
            muni�ao_corente = muni�ao_do_caregador; // Restaura a muni��o da arma atual
        }

        // Disparo
        if (Input.GetButton("Fire1") && Time.time >= tempoDeUltimoTiro + tirospor_minuto)
        {
            atirar();
            tempoDeUltimoTiro = Time.time; // Atualiza o tempo do �ltimo tiro
        }

        // Recarregar
        if (Input.GetKeyDown(KeyCode.R))
        {
            Recarregar();
        }
    }

    void AtualizarEstatisticas()
    {
        dano = gerenciadorDeTrocaDeArmas.dano_de_arma;
        muni�ao_do_caregador = gerenciadorDeTrocaDeArmas.muni�ao_do_caregador;
        muni�ao_guradada = gerenciadorDeTrocaDeArmas.muni�ao_guradada;
        tirospor_minuto = gerenciadorDeTrocaDeArmas.tiros_por_minuto;
    }

    void atirar()
    {
        // Verifica se h� muni��o no carregador
        if (muni�ao_corente <= 0)
        {
            Debug.Log("Sem muni��o no carregador!");
            return;
        }

        RaycastHit acerto;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out acerto))
        {
            Debug.Log($"Acertou: {acerto.transform.name}");

            estatos_do_jogador estatos = acerto.transform.GetComponent<estatos_do_jogador>();
            if (estatos != null)
            {
                estatos.tomar_dano(dano);
            }
        }

        // Reduz muni��o no carregador (muni�ao_corente)
        muni�ao_corente--;
        Debug.Log($"Tiros restantes no carregador: {muni�ao_corente}");

        // Atualiza a muni��o gasta da arma atual
      
    }

    void Recarregar()
    {
        // Calcula o espa�o dispon�vel no carregador
        int espacoDisponivel = muni�ao_do_caregador - muni�ao_corente;

        // Se houver muni��o guardada, recarrega o carregador
        if (muni�ao_guradada > 0 && espacoDisponivel > 0)
        {
            // Determina quantas balas ser�o recarregadas
            int balasParaRecarregar = Mathf.Min(espacoDisponivel, muni�ao_guradada);

            // Atualiza valores de muni��o
            muni�ao_corente += balasParaRecarregar;
            muni�ao_guradada -= balasParaRecarregar;

            Debug.Log($"Recarregou {balasParaRecarregar} balas. Muni��o no carregador: {muni�ao_corente}, Muni��o guardada: {muni�ao_guradada}");
        }
        else if (muni�ao_guradada <= 0)
        {
            Debug.Log("Sem muni��o guardada para recarregar!");
        }
        else
        {
            Debug.Log("Carregador j� est� cheio!");
        }
    }
}
