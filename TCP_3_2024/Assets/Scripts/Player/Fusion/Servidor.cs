using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class Servidor : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner Runner { get; private set; }

    [Header("Network Static Objects")]
    [SerializeField] NetworkObject serverTimerPrefab;
    [SerializeField] NetworkObject matchManagerPrefab;
    [SerializeField] NetworkObject spawnerPrefab;
    [SerializeField] NetworkObject playerSelectionManagerPrefab;
    [SerializeField] NetworkObject winConditionsManagerPrefab;
    [SerializeField] NetworkObject bombPrefab;

    [Header("Server Settings")]
    [SerializeField] private int maxNumberOfPlayers;

    private int roomCount = 1;

    public void StartServer()
    {
        DontDestroyOnLoad(gameObject);
        StartGame();
    }

    async void StartGame(GameMode gameMode = GameMode.AutoHostOrClient)
    {
        if (Runner != null) Destroy(Runner.gameObject);

        var connectionResult = await InitializeNetworkRunner(gameMode, NetAddress.Any(), SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex), null);


        if (!CheckConnectionResult(connectionResult))
        {
            if (connectionResult.ShutdownReason == ShutdownReason.GameIsFull)
            {
                roomCount++;
                Runner.RemoveCallbacks(this);
                StartGame(GameMode.Host);
                return;
            }
        }
        Runner.AddCallbacks(this);
        InitializeServerTimer();
        InitializeSpawner();

        FindObjectOfType<UISearchMatchController>().Initialize();
        //MatchManager.Instance.Initialize();
    }

    private void InitializeServerTimer()
    {
        if (!Runner.IsServer) return;

        NetworkObject serverTimer = Runner.Spawn(serverTimerPrefab, Vector3.zero, Quaternion.identity, PlayerRef.None);
        //Runner.SceneManager.MakeDontDestroyOnLoad(serverTimer.gameObject);

    }

    private void InitializeSpawner()
    {
        if (!Runner.IsServer) return;

        NetworkObject spawner = Runner.Spawn(spawnerPrefab, new Vector3(10, 10, 10), Quaternion.identity, PlayerRef.None);
        if (spawner.GetComponent<Spawner>() is Spawner s)
        {
            Runner.AddCallbacks(s);
            //Runner.SceneManager.MakeDontDestroyOnLoad(s.gameObject);
        }

    }
    public void LoadNextScene(NetworkRunner runner, string sceneName)
    {
        if (!runner.IsSceneAuthority) return;

        SceneRef sceneRef = runner.GetSceneRef(sceneName);

        runner.LoadScene(sceneRef, LoadSceneMode.Single);
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

        //Runner.AddCallbacks(FindObjectOfType<HostManager>());


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
            SessionName = "AAAAA" + roomCount,
            OnGameStarted = onGameStarted,
            SceneManager = sceneManager,
            PlayerCount = maxNumberOfPlayers
        });
    }
    //private void OnGUI()
    //{
    //    if (Runner == null)
    //    {
    //        if (GUI.Button(new Rect(0, 0, 200, 40), "Start Game"))
    //        {
    //            StartGame(GameMode.AutoHostOrClient);
    //            //gameMode = GameMode.AutoHostOrClient;
    //        }
    //        if (GUI.Button(new Rect(0, 40, 200, 40), "Host"))
    //        {
    //            StartGame(GameMode.Host);
    //            //gameMode = GameMode.Host;
    //        }
    //        if (GUI.Button(new Rect(0, 80, 200, 40), "Join"))
    //        {
    //            StartGame(GameMode.Client);
    //            //gameMode = GameMode.Client;
    //        }
    //    }
    //}
    public void Disconect()
    {
        if (Runner == null) return;
        if (!Runner.IsRunning) return;

        Runner.Shutdown();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.ActivePlayers.Count() != maxNumberOfPlayers) return;

        if (!runner.IsSceneAuthority) return;

        SceneRef sceneRef = runner.GetSceneRef("ChangeCharacter");

        runner.LoadScene(sceneRef, LoadSceneMode.Single);
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        if (SceneManager.GetActiveScene().name == "InitialMenu" && shutdownReason == ShutdownReason.DisconnectedByPluginLogic)
        {
            StartGame(GameMode.AutoHostOrClient);
        }

        if (SceneManager.GetActiveScene().name == "ChangeCharacter" && shutdownReason == ShutdownReason.DisconnectedByPluginLogic)
        {
            StartGame(GameMode.AutoHostOrClient);
        }
    }

    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }
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
        if (!runner.IsSceneAuthority) return;

        if (SceneManager.GetActiveScene().name == "TerrainTest")
        {
            Runner.Spawn(matchManagerPrefab, Vector3.zero, Quaternion.identity, PlayerRef.None);
            Runner.Spawn(winConditionsManagerPrefab, Vector3.zero, Quaternion.identity, PlayerRef.None);
            Runner.Spawn(bombPrefab, new Vector3(139.75f, 2.5f, 24.5f), Quaternion.identity, PlayerRef.None);
        }

        if (SceneManager.GetActiveScene().name == "ChangeCharacter")
        {
            Runner.Spawn(playerSelectionManagerPrefab, Vector3.zero, Quaternion.identity, PlayerRef.None);
        }
    }
    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }
}
