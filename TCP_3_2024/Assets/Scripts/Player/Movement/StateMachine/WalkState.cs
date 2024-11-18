using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IState
{
    StateMachine stateMachine;
    public WalkState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    public void Enter()
    {
        Debug.Log("Entrou no estado Andando");
    }

    public void Update()
    {
        stateMachine.TryIdle();
        stateMachine.TryJump();
        
        Debug.Log("Executando estado Andando");
    }

    public void FixedUpdate()
    {
        Vector2 inputs = stateMachine.InputController.MoveAction.ReadValue<Vector2>();
        stateMachine.PlayerMovement.Movement(inputs);
        Debug.Log("Executando estado Andando");
    }

    public void Exit()
    {
        Debug.Log("saiu do estado Andando");
    }
}
