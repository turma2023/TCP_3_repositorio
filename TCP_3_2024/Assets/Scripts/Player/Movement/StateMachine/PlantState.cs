
public class PlantState : IState
{
    StateMachine stateMachine;
    PlayerMovement playerMovement;
    PlayerInputController inputController;
    BombHandler bombHandler;
    public PlantState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        playerMovement = stateMachine.PlayerMovement;
        inputController = stateMachine.InputController;
        bombHandler = stateMachine.BombHandler;
        bombHandler.OnBombPlant += OnBombPlant;
    }
    public void Enter()
    {
        // Play plant animation.
        // Disable Player큦 mouse look.
        // Disable Player큦 Shooting.
    }

    public void Exit()
    {
        // Enable Player큦 movement.
        // Enable Player큦 mouse look.
        // Enable Player큦 Movement.
        // Enablen Player큦 Shooting.
    }

    public void FixedUpdate()
    {

    }

    public void Update()
    {
        if (inputController.Interact.IsPressed())
        {
            bombHandler.TryPlantBomb();
            return;
        }

        if (inputController.Interact.WasReleasedThisFrame())
        {
            stateMachine.ChangeState(stateMachine.StateFactory.Idle);
            bombHandler.ResetTimer();
        }
    }

    private void OnBombPlant()
    {
        stateMachine.ChangeState(stateMachine.StateFactory.Idle);
    }

    ~PlantState()
    {
        bombHandler.OnBombPlant -= OnBombPlant;
    }
}
