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

        if (playerMovement.IsGrounded())
        {
            playerMovement.IsJumping();
        }
        
    }

    public void Exit()
    {
        Debug.Log("saiu do estado Jump");
    }
}
