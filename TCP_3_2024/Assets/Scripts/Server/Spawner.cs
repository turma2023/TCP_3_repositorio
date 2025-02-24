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

    private int numTeam;

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

    private void SetTeam(NetworkObject character1, NetworkObject character2)
    {
        numTeam = UnityEngine.Random.Range(1, 3);
        if (numTeam == 1)
        {
            character1.tag = "Atacante";
            character2.tag = "Atacante";
        }
        if (numTeam == 2)
        {
            character1.tag = "Defensor";
            character2.tag = "Defensor";
        }
    }

    private void SetTeam(NetworkObject character)
    {
        numTeam = UnityEngine.Random.Range(1, 3);

        if (numTeam == 1) character.tag = "Atacante";
        if (numTeam == 2) character.tag = "Defensor";
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

    private void SpawnAllPlayers()
    {
        if (!Runner.IsServer) return;
        List<Transform> beachSidePositionsList = new List<Transform>();
        List<Transform> forestSidePositionsList = new List<Transform>();

        SpawnPositionProvider positionProvider = SpawnPositionProvider.Instance;

        int randomSideChoice = UnityEngine.Random.Range(1, 3);
        NetworkObject spawnedPlayer = null;

        if (positionProvider != null) positionProvider.ProvidePositions(ref beachSidePositionsList, ref forestSidePositionsList);


        foreach (PlayerRef player in Runner.ActivePlayers)
        {
            if (positionProvider != null && SelectedCharactersDictionary.TryGetValue(player.PlayerId, out NetworkObject selectedCharacter))
            {

                int randomBeachSpawnPoint = UnityEngine.Random.Range(0, beachSidePositionsList.Count);
                int randomForestSpawnPoint = UnityEngine.Random.Range(0, forestSidePositionsList.Count);

                if (randomSideChoice % 2 == 0)
                {
                    spawnedPlayer = Runner.Spawn(selectedCharacter, beachSidePositionsList[randomBeachSpawnPoint].position, Quaternion.identity, player);

                    if (spawnedPlayer.TryGetComponent(out BombHandler bombHandler))
                    {
                        bombHandler.Team = TeamSide.Attacker;
                        Debug.LogError($"Team Side Set To : {bombHandler.Team}");
                    }

                    else Debug.LogError($"No Bomb Handler Found On Spawn of player {player.PlayerId}");
                    beachSidePositionsList.Remove(beachSidePositionsList[randomBeachSpawnPoint]);
                }

                else
                {
                    spawnedPlayer = Runner.Spawn(selectedCharacter, forestSidePositionsList[randomForestSpawnPoint].position, Quaternion.identity, player);
                    if (spawnedPlayer.TryGetComponent(out BombHandler bombHandler))
                    {
                        bombHandler.Team = TeamSide.Defender;
                        Debug.LogError($"Team Side Set To : {bombHandler.Team}");
                    }

                    else Debug.LogError($"No Bomb Handler Found On Spawn of player {player.PlayerId}");
                    forestSidePositionsList.Remove(forestSidePositionsList[randomForestSpawnPoint]);
                }

                randomSideChoice++;

                SelectedCharactersDictionary.Remove(player.PlayerId);
            }

            else if (positionProvider == null && SelectedCharactersDictionary.TryGetValue(player.PlayerId, out NetworkObject selectedCharacterException))
            {
                Runner.Spawn(selectedCharacterException, new Vector3(UnityEngine.Random.Range(140, 150), 5f, UnityEngine.Random.Range(150, 121)), Quaternion.identity, player);
                SelectedCharactersDictionary.Remove(player.PlayerId);
            }
        }
    }

    public void SpawnDefaultPlayer(NetworkRunner runner, PlayerRef player)
    {

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

        if (SceneManager.GetActiveScene().name == "TerrainTest")
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
