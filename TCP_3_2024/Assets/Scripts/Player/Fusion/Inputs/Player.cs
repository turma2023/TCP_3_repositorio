using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Fusion;

public class Player : NetworkBehaviour
{
  private NetworkCharacterController _cc;

  private void Awake()
  {
    _cc = GetComponent<NetworkCharacterController>();
  }

  public override void FixedUpdateNetwork()
  {
    if (GetInput(out NetworkInputData data))
    {
        TurnToCameraDirection();
        
        Vector3 moveDirection = (CameraDirection() * data.direction.z + Camera.main.transform.right * data.direction.x).normalized * 5;
      _cc.Move(moveDirection);

    }
  }

    public void TurnToCameraDirection()
    {
        _cc.transform.rotation = Quaternion.LookRotation(CameraDirection());
    }
    private Vector3 CameraDirection()
    {
        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.y = 0; 
        cameraDirection.Normalize();

        return cameraDirection;
    }

}