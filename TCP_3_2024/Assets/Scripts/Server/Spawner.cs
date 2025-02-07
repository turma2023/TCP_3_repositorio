using Fusion;
using UnityEngine;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkObject character1Prefab;
    private NetworkObject character2Prefab;
    private Dictionary<PlayerRef, NetworkObject> selectedCharactersDictionary;

    private void Start()
    {
        selectedCharactersDictionary = new Dictionary<PlayerRef, NetworkObject>();
    }

    public void SetPlayableCharactersPrefabs(NetworkObject character1, NetworkObject character2)
    {
        if (character1 != null) character1Prefab = character1;

        if (character2 != null) character2Prefab = character2;

    }

    public void SetSelectedCharacter(NetworkRunner runner, NetworkObject selectedCharacter)
    {
        foreach (PlayerRef playerRef in runner.ActivePlayers)
        {
            if (playerRef == runner.LocalPlayer)
            {
                if (!selectedCharactersDictionary.ContainsKey(playerRef))
                    selectedCharactersDictionary.Add(playerRef, selectedCharacter);
            }
        }
    }

    public void SetDefaultCharacter(NetworkRunner runner)
    {
        SetSelectedCharacter(runner, character1Prefab);
    }
    public void SpawnSelectedPlayer(NetworkRunner runner, PlayerRef playerRef)
    {
        if (!runner.IsSceneAuthority) return;

        if (selectedCharactersDictionary.ContainsKey(playerRef))
        {
            runner.Spawn(selectedCharactersDictionary[playerRef], new Vector3(UnityEngine.Random.Range(-10, 10), 1, UnityEngine.Random.Range(-10, 10)), Quaternion.identity, playerRef);
            selectedCharactersDictionary.Remove(playerRef);
        }
    }

    private void SpawnAllPlayers(NetworkRunner runner)
    {
        if (runner.ActivePlayers.Count() == 2)
        {
            foreach (PlayerRef playerRef in runner.ActivePlayers)
            {
                SpawnDefaultPlayer(runner, playerRef);
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
        if (!runner.IsSceneAuthority) return;

        if (SceneManager.GetActiveScene().name == "Cena1TestNewServer")
        {
            foreach (PlayerRef playerRef in runner.ActivePlayers)
            {
                SpawnSelectedPlayer(runner, playerRef);
            }
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
