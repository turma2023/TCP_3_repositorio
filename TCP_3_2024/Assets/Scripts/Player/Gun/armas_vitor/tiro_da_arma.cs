using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiro_da_arma : MonoBehaviour
{
    public GerenciadorDeTrocaDeArmas gerenciadorDeTrocaDeArmas;
    private GameObject arma_atual;

    public GameObject cam;

    private int dano;
    public int muniçao_do_caregador;
    public int muniçao_guradada;
    public int muniçao_corente; // A munição que está sendo usada no carregador
    private float tirospor_minuto;
    private float tempoDeUltimoTiro = 0f;

    // Variáveis para armazenar o estado da munição da arma atual
  

    void Start()
    {
        AtualizarEstatisticas();
        // Inicializa a munição da arma atual
    }

    void Update()
    {
        // Atualiza as estatísticas se a arma atual mudar
        if (arma_atual != gerenciadorDeTrocaDeArmas.armaAtual)
        {
          
         

            // Atualiza a arma atual
            arma_atual = gerenciadorDeTrocaDeArmas.armaAtual;
            AtualizarEstatisticas();

            // Restaura a munição da arma anterior se houver
            muniçao_corente = muniçao_do_caregador; // Restaura a munição da arma atual
        }

        // Disparo
        if (Input.GetButton("Fire1") && Time.time >= tempoDeUltimoTiro + tirospor_minuto)
        {
            atirar();
            tempoDeUltimoTiro = Time.time; // Atualiza o tempo do último tiro
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
        muniçao_do_caregador = gerenciadorDeTrocaDeArmas.muniçao_do_caregador;
        muniçao_guradada = gerenciadorDeTrocaDeArmas.muniçao_guradada;
        tirospor_minuto = gerenciadorDeTrocaDeArmas.tiros_por_minuto;
    }

    void atirar()
    {
        // Verifica se há munição no carregador
        if (muniçao_corente <= 0)
        {
            Debug.Log("Sem munição no carregador!");
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

        // Reduz munição no carregador (muniçao_corente)
        muniçao_corente--;
        Debug.Log($"Tiros restantes no carregador: {muniçao_corente}");

        // Atualiza a munição gasta da arma atual
      
    }

    void Recarregar()
    {
        // Calcula o espaço disponível no carregador
        int espacoDisponivel = muniçao_do_caregador - muniçao_corente;

        // Se houver munição guardada, recarrega o carregador
        if (muniçao_guradada > 0 && espacoDisponivel > 0)
        {
            // Determina quantas balas serão recarregadas
            int balasParaRecarregar = Mathf.Min(espacoDisponivel, muniçao_guradada);

            // Atualiza valores de munição
            muniçao_corente += balasParaRecarregar;
            muniçao_guradada -= balasParaRecarregar;

            Debug.Log($"Recarregou {balasParaRecarregar} balas. Munição no carregador: {muniçao_corente}, Munição guardada: {muniçao_guradada}");
        }
        else if (muniçao_guradada <= 0)
        {
            Debug.Log("Sem munição guardada para recarregar!");
        }
        else
        {
            Debug.Log("Carregador já está cheio!");
        }
    }
}
