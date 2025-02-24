using Fusion;
using UnityEngine;

public class StateMachine : NetworkBehaviour
{
    private IState currentState;
    public StateFactory StateFactory { get; private set; }
    public PlayerInputController InputController { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }
    public BombHandler BombHandler { get; private set; }
    public AnimationController animationController {get; private set;}
    private void Start()
    {
        PlayerController player = GetComponent<PlayerController>();
        PlayerMovement = player.PlayerMovement;
        StateFactory = new StateFactory(this);
        InputController = player.PlayerInputController;
        BombHandler = player.BombHandler;
        animationController = GetComponent<AnimationController>();
        ChangeState(StateFactory.Idle);
    }

    public void ChangeState(IState newState)
    {
        //currentState?.Exit();
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
        if (!Object.HasInputAuthority) return;
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void FixedUpdate()
    {   
        if (!Object.HasInputAuthority) return;
        if (currentState != null)
        {
            currentState.FixedUpdate();
        }
    }

    public void TryWalk()
    {

        if (InputController.MoveAction.IsPressed())
        {
            // animationController.PlayWalk(true);

            ChangeState(StateFactory.Walk);
        }

    }

    public void TryIdle()
    {
        if (!InputController.MoveAction.IsPressed())
        {
            ChangeState(StateFactory.Idle);
        }
    }

    public void TryJump()
    {
        if (InputController.JumpAction.WasPressedThisFrame() && PlayerMovement.IsGrounded())
        {
            ChangeState(StateFactory.Jump);
        }
    }

    public void TryPlant()
    {
        if (InputController.Interact.IsPressed() && BombHandler.CanPlant)
        {
            ChangeState(StateFactory.Plant);
        }
    }

    public void TryDefuse()
    {
        if (InputController.Interact.IsPressed() && BombHandler.CanDefuse)
        {
            ChangeState(StateFactory.Defuse); 
        }
    }

}
