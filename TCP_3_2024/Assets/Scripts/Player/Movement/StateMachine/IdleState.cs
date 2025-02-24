using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private StateMachine stateMachine;
    public IdleState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    
    public void Enter()
    {
        // stateMachine.animationController.PlayWalk(false);
        stateMachine.animationController.PlayIdle(true);
    }



    public void Update()
    {
        stateMachine.TryWalk();
        stateMachine.TryJump();
    }

    public void FixedUpdate()
    {
    }

    public void Exit()
    {
    }
}
