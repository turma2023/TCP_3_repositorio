using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;

public class Servidor : MonoBehaviour, INetworkRunnerCallbacks

{
    private NetworkRunner _runner;
    [SerializeField] private NetworkPrefabRef _playerPrefab;

    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();


    private int maxPlayers = 2; // Defina o número máximo de jogadores por sala 
    private int roomCount = 1; // Contador de salas

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        if(_runner != null){
            Destroy(_runner.gameObject);
        }
        _runner = gameObject.AddComponent<NetworkRunner>();
        //!tentativa de criar um novo objeto para receber as informações da sala, problema do pleyer não spawnar na cena
        // _runner = new GameObject("NetworkRunner").AddComponent<NetworkRunner>();  
        // _runner = _runner.GetComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid) {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        // await _runner.StartGame(new StartGameArgs()
        var startGameArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom" + roomCount,
            Scene = scene,
            SceneManager = _runner.gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = maxPlayers
        };

        var result = await _runner.StartGame(startGameArgs);

        if (!result.Ok && result.ShutdownReason == ShutdownReason.GameIsFull) 
        { 
            roomCount++; 
            StartGame(GameMode.AutoHostOrClient); 
        }

    }


    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0,0,200,40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0,40,200,40), "Join"))
            {
                StartGame(GameMode.Client);
            }
            if (GUI.Button(new Rect(0,80,200,40), "Shared")) 
            { 
                StartGame(GameMode.Shared); 
            }
            if (GUI.Button(new Rect(0,120,200,40), "Auto")) 
            { 
                StartGame(GameMode.AutoHostOrClient); 
            }
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // if (runner.IsServer)
        if (_spawnedCharacters.Count < maxPlayers)
        {
            // Create a unique position for the player
            // Debug.Log("esta entrando aqui bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb");
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
        }
        else { Debug.Log("Sala cheia. Não é possível adicionar mais jogadores."); }


        // if (_spawnedCharacters.Count >= maxPlayers)
        // {
        //     roomCount++;
        //     StartGame(GameMode.AutoHostOrClient);
        // }
        // else
        // {
        //     Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-10, 10), 1.5f, UnityEngine.Random.Range(-10, 10));
        //     NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
        //     _spawnedCharacters.Add(player, networkPlayerObject);
        // }


        // if  (runner.IsSharedModeMasterClient && player.IsRealPlayer) 
        // { 
        //     Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-10, 10), 1.5f, UnityEngine.Random.Range(-10, 10)); 
        //     NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
        //     _spawnedCharacters.Add(player, networkPlayerObject); 
        // }


    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }


    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    var data = new NetworkInputData();

    if (Input.GetKey(KeyCode.W))
        data.direction += Vector3.forward;

    if (Input.GetKey(KeyCode.S))
        data.direction += Vector3.back;

    if (Input.GetKey(KeyCode.A))
        data.direction += Vector3.left;

    if (Input.GetKey(KeyCode.D))
        data.direction += Vector3.right;

    input.Set(data);
    }

    



    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){ }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){ }
  }


