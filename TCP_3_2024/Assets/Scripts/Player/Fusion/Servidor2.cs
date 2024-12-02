using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Servidor2 : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private int maxPlayers = 10; // Defina o número máximo de jogadores por sala
    private int roomCount = 1; // Contador de salas
    public string selectedTeam;


    async void StartGame()
    {
        if (_runner != null)
        {
            Destroy(_runner.gameObject);
        }

        _runner = new GameObject("NetworkRunner").AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Adicionar o listener de eventos
        _runner.AddCallbacks(this);
        _runner.AddCallbacks(FindObjectOfType<HostManager>());

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);

        var startGameArgs = new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = "TestRoom" + roomCount,
            Scene = scene,
            SceneManager = _runner.gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = maxPlayers
        };

        var result = await _runner.StartGame(startGameArgs);

        if (!result.Ok && result.ShutdownReason == ShutdownReason.GameIsFull)
        {
            roomCount++;
            StartGame();
        }
        else
        {
            ShowTeamSelection();
        }
    }

    void ShowTeamSelection() { 
        // Mostrar a interface de seleção de time 
        FindObjectOfType<TeamSelection>().Show(); 
    }

    public void SetSelectedTeam(string team) { 
        selectedTeam = team; 
    } 
    public string GetSelectedTeam() { 
        return selectedTeam; 
    }
    

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Start Game"))
            {
                StartGame();
            }
        }
    }


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player){}
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){}
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}
    public void OnInput(NetworkRunner runner, NetworkInput input){}
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){}
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){}
    public void OnConnectedToServer(NetworkRunner runner){}
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason){}
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){}
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){}
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){}
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){}
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){}
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){}
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){}
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){}
    public void OnSceneLoadDone(NetworkRunner runner){}
    public void OnSceneLoadStart(NetworkRunner runner){}
}
