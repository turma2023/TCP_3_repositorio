using UnityEngine;
using System.Collections;

public class TrilhaDaCachoeira : MonoBehaviour
{
    public GameObject trilha_de_agua;
    public float dura�ao_da_trilha = 3f;
    public float fro�a_do_dash = 10f;
    public Transform cameraTransform; // Refer�ncia para a c�mera
    public float distanciaAtras = 1.5f; // Dist�ncia atr�s do jogador

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
            Vector3 posicaoTrilha = transform.position - transform.forward * distanciaAtras; // Coloca a trilha atr�s do jogador
            Quaternion rotacaoTrilha = Quaternion.LookRotation(cameraTransform.forward); // Faz a trilha olhar na dire��o da c�mera

            GameObject currentTrail = Instantiate(trilha_de_agua, posicaoTrilha, rotacaoTrilha);
            Destroy(currentTrail, dura�ao_da_trilha); // Destroi o rastro ap�s a dura��o
        }
    }

    void StartDash()
    {
        rb.AddForce(transform.forward * fro�a_do_dash, ForceMode.Impulse);
        StartCoroutine(AtivarTrilhaPorTempo(2f)); // Ativa a trilha por 2 segundos
    }

    IEnumerator AtivarTrilhaPorTempo(float duracao)
    {
        criandoTrilha = true;
        yield return new WaitForSeconds(duracao);
        criandoTrilha = false;
    }
}
