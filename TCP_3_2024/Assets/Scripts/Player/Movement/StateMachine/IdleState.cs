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
        stateMachine.AnimationController.PlayIdle(true);
    }

    public void Update()
    {
        stateMachine.TryWalk();
        stateMachine.TryJump();
        stateMachine.TryPlant();
        stateMachine.TryDefuse();
    }

    public void FixedUpdate()
    {
    }

    public void Exit()
    {
    }
}
