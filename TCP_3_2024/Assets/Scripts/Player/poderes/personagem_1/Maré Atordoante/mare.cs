using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mare : MonoBehaviour
{
    public float speed = 10f; // Velocidade da onda
    public float maxDistance = 20f; // Dist�ncia m�xima que a onda viaja
    public float stunDuration = 3f; // Tempo de stun nos inimigos
    public LayerMask groundLayer; // Camada do ch�o

    private Vector3 startPosition; // Posi��o inicial da onda
    private bool isMoving = true; // Controla se a onda est� se movendo

    void Start()
    {
        startPosition = transform.position; // Guarda a posi��o inicial
        AlignToGround(); // Alinha a onda ao ch�o no in�cio
    }

    void Update()
    {
        if (isMoving)
        {
            // Move a onda para frente
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            // Alinha a onda ao ch�o a cada frame
            AlignToGround();

            // Verifica se a onda atingiu a dist�ncia m�xima
            if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
            {
                Destroy(gameObject); // Destroi a onda
            }
        }
    }

    void AlignToGround()
    {
        // Lan�a um Raycast para baixo para detectar o ch�o
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            // Ajusta a posi��o da onda para ficar sobre o ch�o
            transform.position = hit.point;

            // Ajusta a rota��o da onda para seguir a normal do ch�o
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Colidiu com: " + other.gameObject.name); // Verifica se a colis�o est� sendo detectada

        if (other.gameObject.tag != gameObject.tag)
        {
            Debug.Log("Tag do outro objeto n�o � a mesma, aplicando stun!");
            PlayerMovement enemy = other.GetComponent<PlayerMovement>();
            if (enemy != null)
            {
                enemy.Stun(stunDuration);
            }
        }
    }

}
