using UnityEngine;
using Fusion;
using System.Collections.Generic;
using Fusion.Sockets;
using Photon.Realtime;

public class HostManager : NetworkBehaviour, INetworkRunnerCallbacks
{
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    [SerializeField] private NetworkPrefabRef _playerPrefab;



    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        SpawnPlayer(runner, player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }

        if (Object.HasStateAuthority && player == Object.InputAuthority)
        {
            TransferHost(runner);
        }

    }

    private void SpawnPlayer(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.Count < runner.SessionInfo.PlayerCount)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10));
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            _spawnedCharacters.Add(player, networkPlayerObject);
            Debug.Log("Host Manager Player Joined");
            Debug.Log("Player ID: " + player.PlayerId);

            foreach (var character in _spawnedCharacters)
            {
                Debug.Log("Joined ID: " + character.Key.PlayerId);
            }


        }
        else
        {
            Debug.Log("Sala cheia. Não é possível adicionar mais jogadores.");
        }
    }

    private void ResetPlayer()
    {

    }

    public void TransferHost(NetworkRunner runner)
    {
        // Encontrar um novo jogador para ser o host
        Debug.Log("ta no Tranfer");
        foreach (var player in runner.ActivePlayers)
        {
            if (player != Object.InputAuthority)
            {
                runner.SetPlayerObject(player, runner.GetPlayerObject(player));
                Debug.Log("Novo host: " + player);
                break;
            }
        }
    }


    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner)
    {


    }
    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("Player Count: " + runner.SessionInfo.PlayerCount);
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }






}
