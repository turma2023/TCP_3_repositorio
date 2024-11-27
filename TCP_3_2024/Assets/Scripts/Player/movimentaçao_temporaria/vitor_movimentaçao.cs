using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vitor_movimentaçao : MonoBehaviourPunCallbacks
{
    public float velocidadeDoJogador = 5f;
    public float sensibilidadeDoMouse = 2f;
    public float velocidadeDePulo = 5f;

    public Rigidbody rb;
    public GameObject camera;
    public Camera cameraJogador;
    public LayerMask chao;

    private float rotacaoX = 0f;
    private bool estaNoChao;

    public PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();

        // Desativa os componentes de controle e a câmera para jogadores remotos
        if (!view.IsMine)
        {
            cameraJogador.gameObject.SetActive(false); // Desativa a câmera do jogador remoto
            rb.isKinematic = true; // Desativa a física para jogadores remotos
        }
        else
        {
            rb.isKinematic = false; // Ativa a física para o jogador local
        }
    }

    void Update()
    {
        // Garante que apenas o jogador local controle o objeto
        if (view.IsMine)
        {
            MoverCamera();
            MoverJogador();
            VerificarPulo();
        }
    }

    void MoverCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeDoMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeDoMouse;

        // Rotaciona o jogador no eixo Y
        transform.Rotate(0, mouseX, 0);

        // Rotaciona a câmera no eixo X
        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);

        camera.transform.localRotation = Quaternion.Euler(rotacaoX, 0, 0);
    }

    void MoverJogador()
    {
        float movimentoHorizontal = Input.GetAxis("Horizontal");
        float movimentoVertical = Input.GetAxis("Vertical");

        Vector3 direcao = transform.right * movimentoHorizontal + transform.forward * movimentoVertical;
        Vector3 velocidade = direcao.normalized * velocidadeDoJogador;
        Vector3 velocidadeVertical = new Vector3(0, rb.velocity.y, 0);

        rb.velocity = velocidade + velocidadeVertical;
    }

    void VerificarPulo()
    {
        estaNoChao = Physics.CheckSphere(transform.position, 0.2f, chao);

        if (estaNoChao && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * velocidadeDePulo, ForceMode.Impulse);
        }
    }
}
