using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Servidor2 : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner Runner { get; private set; }
    public static Servidor2 Instance { get; private set; }
    private int maxPlayers = 10; // Defina o número máximo de jogadores por sala
    private int roomCount = 1; // Contador de salas
    private GameMode gameMode = GameMode.AutoHostOrClient;

    [SerializeField] public NetworkObject ballPrefab;


    async void StartGame(GameMode gameMode)
    {
        Debug.Log("Game Started");

        if (Runner != null)
        {
            Destroy(Runner.gameObject);
        }

        Runner = new GameObject("NetworkRunner").AddComponent<NetworkRunner>();
        Runner.ProvideInput = true;

        // Adicionar o listener de eventos
        Runner.AddCallbacks(this);
        Runner.AddCallbacks(FindObjectOfType<HostManager>());

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);

        var startGameArgs = new StartGameArgs()
        {
            // ObjectProvider = new PooledNetworkObjectProvider(), //!aqui para ativar o pool
            GameMode = gameMode,
            SessionName = "TestRoom2" + roomCount,
            Scene = scene,
            SceneManager = Runner.gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = maxPlayers
        };

        var result = await Runner.StartGame(startGameArgs);

        if (!result.Ok && result.ShutdownReason == ShutdownReason.GameIsFull)
        {
            roomCount++;
            StartGame(gameMode);
        }

        MatchManager.Instance.Initialize();
    }

    private void Awake()
    {
        if (!Instance) Instance = this;
    }
    private void OnGUI()
    {
        if (Runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Start Game"))
            {
                StartGame(GameMode.AutoHostOrClient);
                gameMode = GameMode.AutoHostOrClient;
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
                gameMode = GameMode.Host;
            }
            if (GUI.Button(new Rect(0, 80, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
                gameMode = GameMode.Client;
            }
        }
    }


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }
    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    public void OnEnable()
    {

    }
}
