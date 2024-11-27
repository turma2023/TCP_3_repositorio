using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class segir_camera : MonoBehaviour
{
    public Transform alvo;             // O objeto a ser seguido (ex: o jogador)
    public float suavizacao = 0.1f;    // Suaviza��o do movimento da c�mera
    public Vector3 offset;             // Dist�ncia da c�mera em rela��o ao jogador

    private Transform cameraTransform;  // Transform da c�mera
    private Transform playerTransform; // Transform do jogador

    private float rotacaoX = 0f;       // Rota��o X da c�mera (para controlar a inclina��o)
    private float rotacaoY = 0f;       // Rota��o Y da c�mera (para controlar a rota��o do jogador)

    private void Start()
    {
        if (alvo == null)
        {
            alvo = GameObject.FindWithTag("Player").transform; // Busca o jogador automaticamente
        }
        cameraTransform = Camera.main.transform; // Obt�m o transform da c�mera principal
        playerTransform = alvo; // Define o transform do jogador
    }

    private void Update()
    {
        if (alvo != null)
        {
            // Captura a entrada do mouse para a rota��o da c�mera (controle da inclina��o)
            rotacaoX -= Input.GetAxis("Mouse Y");
            rotacaoY += Input.GetAxis("Mouse X");

            // Restringe a rota��o X para n�o inverter a c�mera
            rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);

            // Atualiza a rota��o da c�mera com base nos movimentos do mouse
            Quaternion rotacaoFinal = Quaternion.Euler(rotacaoX, rotacaoY, 0);
            cameraTransform.rotation = rotacaoFinal;

            // Aplica a posi��o da c�mera com suaviza��o
            Vector3 posicaoDesejada = playerTransform.position + offset;
            transform.position = Vector3.Lerp(transform.position, posicaoDesejada, suavizacao);
        }
    }
}