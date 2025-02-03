using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFactory
{

    public IState Idle { get; private set; }
    public IState Walk {get; private set; }
    public IState Jump { get; private set; }
    public IState Plant { get; private set; }
    public IState Defuse { get; private set; }

    public StateFactory(StateMachine stateMachine)
    {
        Idle = new IdleState(stateMachine);
        Walk = new WalkState(stateMachine);
        Jump = new JumpState(stateMachine);
        Plant = new JumpState(stateMachine);
        Defuse = new JumpState(stateMachine);
    }

}
