
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
        // Disable Player�s mouse look.
        // Disable Player�s Shooting.
    }

    public void Exit()
    {
        // Enable Player�s movement.
        // Enable Player�s mouse look.
        // Enable Player�s Movement.
        // Enablen Player�s Shooting.
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
