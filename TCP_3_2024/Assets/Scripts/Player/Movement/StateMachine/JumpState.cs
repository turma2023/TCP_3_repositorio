using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{
    StateMachine stateMachine;
    PlayerMovement playerMovement;
    public JumpState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        playerMovement = stateMachine.GetComponent<PlayerMovement>();
    }
    public void Enter()
    {
    }

    public void Update()
    {
        stateMachine.TryIdle();
        stateMachine.TryWalk();

    }

    public void FixedUpdate()
    {


        if (playerMovement.IsGrounded())
        {
            playerMovement.IsJumping();
        }
        
    }

    public void Exit()
    {
    }
}
