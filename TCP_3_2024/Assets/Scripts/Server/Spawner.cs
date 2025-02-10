using Fusion;
using UnityEngine;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Spawner : NetworkBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkObject character1Prefab;
    [SerializeField] private NetworkObject character2Prefab;
    public Dictionary<int, NetworkObject> SelectedCharactersDictionary { get; set; }


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        SelectedCharactersDictionary = new Dictionary<int, NetworkObject>();
    }

    public void SetPlayableCharactersPrefabs(NetworkObject character1, NetworkObject character2)
    {
        if (character1 != null) character1Prefab = character1;

        if (character2 != null) character2Prefab = character2;

    }

    public void SetSelectedCharacter(PlayerRef playerRef, int selectedCharacter)
    {
        if (!Runner.IsServer) return;

        if (selectedCharacter == 1) RegisterSelectedCharacter(playerRef, character1Prefab);
        if (selectedCharacter == 2) RegisterSelectedCharacter(playerRef, character2Prefab);
    }

    private void RegisterSelectedCharacter(PlayerRef playerRef, NetworkObject selectedCharacter)
    {
        SelectedCharactersDictionary.TryAdd(playerRef.PlayerId, selectedCharacter);
    }

    //[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    //public void RPC_SetDefaultCharacter(NetworkRunner runner)
    //{
    //    RPC_SetSelectedCharacter(runner, character1Prefab);
    //}
    public void SpawnSelectedPlayer(NetworkRunner runner, PlayerRef playerRef)
    {
        if (!runner.IsServer) return;
        //Debug.Log(selectedCharactersDictionary.Count);

        //foreach (PlayerRef player in runner.ActivePlayers)
        //{
        //    if (selectedCharactersDictionary.ContainsKey(player))
        //    {
        //        runner.Spawn(selectedCharactersDictionary[player], new Vector3(UnityEngine.Random.Range(-10, 10), 1, UnityEngine.Random.Range(-10, 10)), Quaternion.identity, playerRef);
        //        selectedCharactersDictionary.Remove(player);
        //    }
        //}

    }

    //public override void Spawned()
    //{
    //    if (PlayerSelection.selected == 1) Spawn(character1Prefab);
    //    if (PlayerSelection.selected == 2) Spawn(character2Prefab);

    //    void Spawn(NetworkObject prefab)
    //    {
    //        if (prefab == null)
    //        {
    //            Debug.Log("Prefab null on spawn");
    //        }

    //        foreach (PlayerRef player in Runner.ActivePlayers)
    //        {
    //            Runner.Spawn(prefab, new Vector3(UnityEngine.Random.Range(-10, 10), 1, UnityEngine.Random.Range(-10, 10)), Quaternion.identity, player);
    //        }
    //    }

    //}
    private void SpawnAllPlayers()
    {
        if (!Runner.IsServer) return;

        foreach (PlayerRef player in Runner.ActivePlayers)
        {

            if (SelectedCharactersDictionary.TryGetValue(player.PlayerId, out NetworkObject selectedCharacter))
            {
                Runner.Spawn(selectedCharacter, new Vector3(UnityEngine.Random.Range(-10, 10), 1, UnityEngine.Random.Range(-10, 10)), Quaternion.identity, player);
                SelectedCharactersDictionary.Remove(player.PlayerId);
            }
        }
    }
    public void SpawnDefaultPlayer(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            runner.Spawn(character1Prefab, new Vector3(UnityEngine.Random.Range(-10, 10), 1, UnityEngine.Random.Range(-10, 10)), Quaternion.identity, player);
        }
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }
    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }



    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if (SceneManager.GetActiveScene().name == "Cena1TestNewServer")
        {
            SpawnAllPlayers();
        }
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

}
