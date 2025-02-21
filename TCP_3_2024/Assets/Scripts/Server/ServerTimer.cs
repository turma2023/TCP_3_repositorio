using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;

public class ServerTimer : NetworkBehaviour, INetworkRunnerCallbacks
{
    [Networked] public TickTimer Timer { get; private set; }
    public event Action OnTimerExpired;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void StartTimer(float timerDuration)
    {
        if (Runner.IsServer)
        {
            Timer = TickTimer.CreateFromSeconds(Runner, timerDuration);
        }
    }

    public bool HasTimerExpired()
    {
        return Timer.Expired(Runner);
    }
    public void Update()
    {
        if (Timer.Expired(Runner))
        {
            Timer = TickTimer.None;
            OnTimerExpired?.Invoke();
        }
    }
    public float? RemainingTime()
    {
        return Timer.RemainingTime(Runner);
    }

    public bool IsActive()
    {
        return Timer.IsRunning;
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
}
