using System.Collections;
using UnityEngine;

public class InundacaoDaCascata : MonoBehaviour
{
    public float stunDuration = 2f; // Dura��o do stun
    public float spideeffect_duratin = 2f;

    [Header("Par�metros de Crescimento")]
    public float growthSpeed = 1f;  // Velocidade de crescimento
    public float maxSize = 2f;      // Tamanho m�ximo
    public float minSize = 0.5f;    // Tamanho m�nimo
    public int pulseCount = 3;      // Quantidade de pulsos (cresce e diminui)

    private bool isPulsing = false; // Para evitar m�ltiplas execu��es

    private void Start()
    {
        StartCoroutine(PulseEffect()); // Inicia o sistema de pulsos automaticamente
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != gameObject.tag) // Se a tag for diferente
        {
            Debug.Log("Tag diferente! Aplicando stun...");
            PlayerMovement enemy = other.GetComponent<PlayerMovement>();
            if (enemy != null)
            {
                enemy.Stun(stunDuration);
            }
        }
        else 
        {
            Debug.Log("Tag igual! Aplicando speedbuff...");
            PlayerMovement enemy = other.GetComponent<PlayerMovement>();
            if (enemy != null)
            {
                enemy.speedbuff(spideeffect_duratin);
            }
        }
    }

    private IEnumerator PulseEffect()
    {
        isPulsing = true;
        Vector3 originalScale = transform.localScale;

        for (int i = 0; i < pulseCount; i++)
        {
            // Crescimento do objeto at� o tamanho m�ximo
            yield return StartCoroutine(ScaleOverTime(originalScale, maxSize, growthSpeed));

            // Diminui��o do objeto at� o tamanho m�nimo
            yield return StartCoroutine(ScaleOverTime(transform.localScale, minSize, growthSpeed));
        }

        // Ap�s completar os pulsos, o objeto desaparece
        gameObject.SetActive(false);
    }

    private IEnumerator ScaleOverTime(Vector3 startScale, float targetScale, float speed)
    {
        Vector3 target = new Vector3(targetScale, targetScale, targetScale);
        float progress = 0f;

        while (progress < 1f)
        {
            // Lerp suave entre o tamanho inicial e o alvo
            transform.localScale = Vector3.Lerp(startScale, target, progress);
            progress += Time.deltaTime * speed;
            yield return null;
        }

        // Garante que a escala final seja exatamente o tamanho alvo
        transform.localScale = target;
    }

    private void Update()
    {
        // Mant�m o objeto preso ao objeto ao qual est� associado
        if (transform.parent != null)
        {
            transform.position = transform.parent.position;
        }
    }
}
