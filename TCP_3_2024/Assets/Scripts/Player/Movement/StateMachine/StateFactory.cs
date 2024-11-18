using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFactory
{

    public IState Idle { get; private set; }
    public IState Walk {get; private set; }
    public IState Jump { get; private set; }

    public StateFactory(StateMachine stateMachine)
    {
        this.Idle = new IdleState(stateMachine);
        this.Walk = new WalkState(stateMachine);
        this.Jump = new JumpState(stateMachine);
    }

}
