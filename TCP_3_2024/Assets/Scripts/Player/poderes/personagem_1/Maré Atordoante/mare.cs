using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mare : NetworkBehaviour
{
    public float speed = 10f; // Velocidade da onda
    public float maxDistance = 20f; // Distância máxima que a onda viaja
    public float stunDuration = 3f; // Tempo de stun nos inimigos
    public LayerMask groundLayer; // Camada do chão

    private Vector3 startPosition; // Posição inicial da onda
    private bool isMoving = true; // Controla se a onda está se movendo

    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            startPosition = transform.position; // Guarda a posição inicial
            AlignToGround(); // Alinha a onda ao chão no início
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner.IsServer && isMoving)
        {
            MoveWave();
        }
    }

    void MoveWave()
    {
        // Move a onda para frente na direção correta
        transform.position += transform.forward * speed * Runner.DeltaTime;

        // Alinha a onda ao chão a cada frame
        AlignToGround();

        // Verifica se a onda atingiu a distância máxima
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Runner.Despawn(Object); // Remove a onda da rede
        }
    }

    void AlignToGround()
    {
        // Lança um Raycast para baixo para detectar o chão
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            // Ajusta a posição da onda para ficar sobre o chão
            transform.position = hit.point;

            // Ajusta a rotação da onda para seguir a normal do chão
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Colidiu com: " + other.gameObject.name); // Verifica se a colisão está sendo detectada

        if (other.gameObject.tag != gameObject.tag)
        {
            Debug.Log("Tag do outro objeto não é a mesma, aplicando stun!");
            PlayerMovement enemy = other.GetComponent<PlayerMovement>();
            if (enemy != null)
            {
                enemy.Stun(stunDuration);
            }
        }
    }
}
