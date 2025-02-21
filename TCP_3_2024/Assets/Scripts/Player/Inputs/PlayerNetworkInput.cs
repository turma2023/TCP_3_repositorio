using Fusion;
using UnityEngine;

public struct PlayerNetworkInput : INetworkInput
{
    public Vector3 MoveDirection {  get; set; }
    public int Tick {  get; set; }
}
