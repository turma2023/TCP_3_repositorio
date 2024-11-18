using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump
{
    LayerMask layerMask = LayerMask.GetMask("Grounded");

    StateMachine stateMachine;

    public PlayerJump(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    
    public void IsJumping()
    {
        stateMachine.PlayerMovement.rigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }
    
    public bool IsGrounded()
    {

        RaycastHit hit;

        if (Physics.Raycast(stateMachine.transform.position, stateMachine.transform.TransformDirection(Vector3.down), out hit, 1.01f, layerMask))
        {
            Debug.DrawRay(stateMachine.transform.position, stateMachine.transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow); 
            Debug.Log("Está no chao"); 
            return true;
        }
          
        return false;
        
    }
}
