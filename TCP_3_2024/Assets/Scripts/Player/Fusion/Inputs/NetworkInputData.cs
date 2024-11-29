using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
  public Vector3 direction;

  public Vector2 move; 
  public bool jump;

}