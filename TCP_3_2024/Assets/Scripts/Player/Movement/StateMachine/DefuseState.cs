
public class DefuseState : IState
{
    StateMachine stateMachine;
    PlayerMovement playerMovement;
    BombHandler bombHandler;
    PlayerInputController inputController;

    public DefuseState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        playerMovement = stateMachine.PlayerMovement;
        bombHandler = stateMachine.BombHandler;
        inputController = stateMachine.InputController;
        bombHandler.OnBombDefuse += OnBombDefuse;
    }
    public void Enter()
    {
        // Play defusing animation.
        // Disable Player Movement.
        // Disable Player Looking.
        // Disable Player Shooting.
    }

    public void Exit()
    {
        // Enable Player Movement.
        // Enable Player Shooting.
        // Enable Player Looking.
    }

    public void FixedUpdate()
    {

    }

    public void Update()
    {
        // Update Bomb Handler.
        // Try exit state.
        if (inputController.Interact.IsPressed())
        {
            bombHandler.TryDefuseBomb();
            return;
        }
        
        if (inputController.Interact.WasReleasedThisFrame())
        {
            stateMachine.ChangeState(stateMachine.StateFactory.Idle);
            bombHandler.ResetTimer();
            bombHandler.UpdateDefusing(false);
        }
    }

    private void OnBombDefuse()
    {
        stateMachine.ChangeState(stateMachine.StateFactory.Idle);
    }

    ~DefuseState()
    {
        bombHandler.OnBombDefuse -= OnBombDefuse;
    }
}
