using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class conequiçao_do_servidor : MonoBehaviourPunCallbacks
{
    public InputField roomNameInput;  // Campo de entrada para o nome da sala
    public Button createButton;       // Botão de criar sala
    public Button joinButton;         // Botão de entrar em uma sala

    void Start()
    {
        // Conectar ao Photon
        PhotonNetwork.ConnectUsingSettings();

        // Adiciona funções aos botões
        createButton.onClick.AddListener(CreateRoom);
        joinButton.onClick.AddListener(JoinRoom);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado ao servidor Photon!");
    }

    // Função para criar uma sala com o nome inserido
    void CreateRoom()
    {
        string roomName = roomNameInput.text; // Obtém o nome da sala do InputField
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogWarning("Nome da sala não pode ser vazio!");
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10; // Define o limite de jogadores na sala
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    // Função para entrar em uma sala com o nome inserido
    void JoinRoom()
    {
        string roomName = roomNameInput.text; // Obtém o nome da sala do InputField
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogWarning("Nome da sala não pode ser vazio!");
            return;
        }

        PhotonNetwork.JoinRoom(roomName); // Tenta entrar na sala
    }

    // Callback caso a criação da sala seja bem-sucedida
    public override void OnCreatedRoom()
    {
        Debug.Log("Sala criada com sucesso!");

        // Carrega a cena "cena2" quando a sala for criada
        PhotonNetwork.LoadLevel("cena2");
    }

    // Callback caso o jogador entre com sucesso na sala
    public override void OnJoinedRoom()
    {
        Debug.Log("Entrou na sala com sucesso!");

        // Carrega a cena "cena2" quando o jogador entra na sala
        PhotonNetwork.LoadLevel("cena2");
    }

    // Callback caso a tentativa de entrar em uma sala falhe
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Falha ao entrar na sala: " + message);
    }
}
