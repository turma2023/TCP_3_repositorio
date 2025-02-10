using UnityEngine;
using System.Collections;

public class TrilhaDaCachoeira : MonoBehaviour
{
    public GameObject trilha_de_agua;
    public float duraçao_da_trilha = 3f;
    public float froça_do_dash = 10f;
    public Transform cameraTransform; // Referência para a câmera
    public float distanciaAtras = 1.5f; // Distância atrás do jogador

    private Rigidbody rb;
    private bool criandoTrilha = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartDash();
        }
    }

    private void FixedUpdate()
    {
        if (criandoTrilha)
        {
            Vector3 posicaoTrilha = transform.position - transform.forward * distanciaAtras; // Coloca a trilha atrás do jogador
            Quaternion rotacaoTrilha = Quaternion.LookRotation(cameraTransform.forward); // Faz a trilha olhar na direção da câmera

            GameObject currentTrail = Instantiate(trilha_de_agua, posicaoTrilha, rotacaoTrilha);
            Destroy(currentTrail, duraçao_da_trilha); // Destroi o rastro após a duração
        }
    }

    void StartDash()
    {
        rb.AddForce(transform.forward * froça_do_dash, ForceMode.Impulse);
        StartCoroutine(AtivarTrilhaPorTempo(2f)); // Ativa a trilha por 2 segundos
    }

    IEnumerator AtivarTrilhaPorTempo(float duracao)
    {
        criandoTrilha = true;
        yield return new WaitForSeconds(duracao);
        criandoTrilha = false;
    }
}
