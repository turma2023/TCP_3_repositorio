using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class WalkState : NetworkBehaviour, IState
{
    StateMachine stateMachine;
    public WalkState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
<<<<<<< Updated upstream
        Debug.Log("Entrou no estado Andando");
=======
        Debug.Log("entrou no walkstate");
        stateMachine.animationController.PlayWalk(true);
>>>>>>> Stashed changes
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
        // var data = new NetworkInputData();
        // data.direction = inputs;
        // stateMachine.PlayerMovement.Movement(data.direction);
        
        
    }

    public void Exit()
    {
        Debug.Log("saiu do estado Andando");
    }
}
