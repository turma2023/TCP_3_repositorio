using System.Collections;
using UnityEngine;

public class JumpState : IState
{
    StateMachine stateMachine;
    PlayerMovement playerMovement;
    float jumpAnimationDelay = 0.1f; 
    public JumpState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        playerMovement = stateMachine.GetComponent<PlayerMovement>();
    }
    public void Enter()
    {
        stateMachine.animationController.PlayJump(true);
        stateMachine.StartCoroutine(TriggerJumpAnimation());
    }

    public void Update()
    {
        stateMachine.TryIdle();
        stateMachine.TryWalk();
    }

    public void FixedUpdate()
    {
        
    }

    IEnumerator TriggerJumpAnimation()
    {
        yield return new WaitForSeconds(jumpAnimationDelay);
        playerMovement.IsJumping();
    }


    public void Exit()
    {
    }
}
