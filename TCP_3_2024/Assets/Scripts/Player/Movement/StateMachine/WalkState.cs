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
    }

    public void Update()
    {
        stateMachine.TryIdle();
        stateMachine.TryJump();

    }

    public void FixedUpdate()
    {
        Vector2 inputs = stateMachine.InputController.MoveAction.ReadValue<Vector2>();

        stateMachine.PlayerMovement.Movement(inputs);
    }

    public void Exit()
    {
    }
}
