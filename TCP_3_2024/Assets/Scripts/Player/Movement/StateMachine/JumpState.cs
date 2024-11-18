using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{
    StateMachine stateMachine;
    PlayerJump playerJump;
    public JumpState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        playerJump = new PlayerJump(stateMachine);
    }
    public void Enter()
    {
        Debug.Log("Entrou no estado Jump");
    }

    public void Update()
    {
        stateMachine.TryIdle();
        stateMachine.TryWalk();

        Debug.Log("Executando estado Jump");
    }

    public void FixedUpdate()
    {

        Debug.Log("Executando estado Jump");
        if (playerJump.IsGrounded())
        {
            playerJump.IsJumping();
        }

        // if (IsGrounded())
        // {
        //     stateMachine.PlayerMovement.rigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);            
        // }
    }

    // private bool IsGrounded()
    // {
    //
    //     RaycastHit hit;
    //
    //     if (Physics.Raycast(stateMachine.transform.position, stateMachine.transform.TransformDirection(Vector3.down), out hit, 1.1f, layerMask))
    //     {
    //         Debug.DrawRay(stateMachine.transform.position, stateMachine.transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow); 
    //         Debug.Log("Está no chao"); 
    //         return true;
    //     }
    //       
    //     return false;
    //     
    // }

    public void Exit()
    {
        Debug.Log("saiu do estado Jump");
    }
}
