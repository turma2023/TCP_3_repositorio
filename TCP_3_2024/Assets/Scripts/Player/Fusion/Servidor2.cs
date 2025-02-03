using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Unity.VisualScripting;

public class Servidor2 : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner Runner { get; private set; }
    [SerializeField] private NetworkPrefabRef character1Prefab;
    [SerializeField] private NetworkPrefabRef character2Prefab;
    [SerializeField] private int maxPlayers; // Defina o número máximo de jogadores por sala
    //private int roomCount = 1; // Contador de salas
    //private GameMode gameMode = GameMode.AutoHostOrClient;

    [SerializeField] public NetworkObject ballPrefab;

    async void StartGame(GameMode gameMode)
    {
        if (Runner != null) Destroy(Runner.gameObject);

        var connectionResult = await InitializeNetworkRunner(gameMode, NetAddress.Any(), SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex), null);

        if (!CheckConnectionResult(connectionResult))
        {
            StartGame(gameMode);
            return;
        }

        FindObjectOfType<UISearchMatchController>().Initialize();
        //MatchManager.Instance.Initialize();
    }

    private bool CheckConnectionResult(StartGameResult connectionResult)
    {
        if (!connectionResult.Ok && connectionResult.ShutdownReason == ShutdownReason.GameIsFull)
        {
            //roomCount++;
            return false;
        }

        return true;
    }

    public virtual Task<StartGameResult> InitializeNetworkRunner(GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> onGameStarted)
    {
        Runner = new GameObject("Network Runner").AddComponent<NetworkRunner>();

        Spawner spawner = Runner.AddComponent<Spawner>();
        spawner.SetPlayableCharactersPrefabs(character1Prefab, character2Prefab);

        Runner.AddCallbacks(this);
        Runner.AddCallbacks(FindObjectOfType<HostManager>());
        Runner.AddCallbacks(spawner);

        var sceneManager = Runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            sceneManager = Runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        Runner.ProvideInput = true;

        return Runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = "Test Room",
            OnGameStarted = onGameStarted,
            SceneManager = sceneManager,
            PlayerCount = maxPlayers
        });
    }

    private void OnGUI()
    {
        if (Runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Start Game"))
            {
                StartGame(GameMode.AutoHostOrClient);
                //gameMode = GameMode.AutoHostOrClient;
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
                //gameMode = GameMode.Host;
            }
            if (GUI.Button(new Rect(0, 80, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
                //gameMode = GameMode.Client;
            }
        }
    }


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.ActivePlayers.Count() != maxPlayers) return;

        if (!runner.IsSceneAuthority) return;

        // Unload current active scene.
        runner.UnloadScene(SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex));

        // Load next specified scene.
        SceneRef sceneRef = runner.GetSceneRef("Cena1TestNewServer");
        runner.LoadScene(sceneRef, LoadSceneMode.Single);
    }
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
}
