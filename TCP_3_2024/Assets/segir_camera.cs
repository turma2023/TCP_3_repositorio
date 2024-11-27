using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class segir_camera : MonoBehaviour
{
    public Transform alvo;             // O objeto a ser seguido (ex: o jogador)
    public float suavizacao = 0.1f;    // Suavização do movimento da câmera
    public Vector3 offset;             // Distância da câmera em relação ao jogador

    private Transform cameraTransform;  // Transform da câmera
    private Transform playerTransform; // Transform do jogador

    private float rotacaoX = 0f;       // Rotação X da câmera (para controlar a inclinação)
    private float rotacaoY = 0f;       // Rotação Y da câmera (para controlar a rotação do jogador)

    private void Start()
    {
        if (alvo == null)
        {
            alvo = GameObject.FindWithTag("Player").transform; // Busca o jogador automaticamente
        }
        cameraTransform = Camera.main.transform; // Obtém o transform da câmera principal
        playerTransform = alvo; // Define o transform do jogador
    }

    private void Update()
    {
        if (alvo != null)
        {
            // Captura a entrada do mouse para a rotação da câmera (controle da inclinação)
            rotacaoX -= Input.GetAxis("Mouse Y");
            rotacaoY += Input.GetAxis("Mouse X");

            // Restringe a rotação X para não inverter a câmera
            rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);

            // Atualiza a rotação da câmera com base nos movimentos do mouse
            Quaternion rotacaoFinal = Quaternion.Euler(rotacaoX, rotacaoY, 0);
            cameraTransform.rotation = rotacaoFinal;

            // Aplica a posição da câmera com suavização
            Vector3 posicaoDesejada = playerTransform.position + offset;
            transform.position = Vector3.Lerp(transform.position, posicaoDesejada, suavizacao);
        }
    }
}