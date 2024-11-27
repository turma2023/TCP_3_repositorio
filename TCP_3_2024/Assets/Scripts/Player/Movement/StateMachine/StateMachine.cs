using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class StateMachine : NetworkBehaviour
{
    private IState currentState;
    public StateFactory factory { get; private set; }
    public PlayerInputController InputController { get; private set; }
    
    public PlayerMovement PlayerMovement { get; private set; }
    private void Start()
    {
        PlayerController player = GetComponent<PlayerController>();
        PlayerMovement = player.PlayerMovement;
        factory = new StateFactory(this);
        ChangeState(factory.Idle);
        InputController = player.PlayerInputController;
    }
    
    public void ChangeState(IState newState)
    {   
        // currentState?.Exit();
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        // currentState?.Execute();
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.FixedUpdate();
        }
    }

    public void TryWalk()
    {

        if (InputController.MoveAction.IsPressed())
        {
            ChangeState(factory.Walk);
        }
        
    }

    public void TryIdle()
    {
        if (!InputController.MoveAction.IsPressed())
        {
            ChangeState(factory.Idle);
        }
    }

    public void TryJump()
    {
        if(InputController.JumpAction.WasPressedThisFrame()){
            ChangeState(factory.Jump);
        }
    }
    
}
