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
        Debug.Log("Entrou no estado Parado");
    }



    public void Update()
    {
        stateMachine.TryWalk();
        Debug.Log("Executando estado Parado");
    }

    public void FixedUpdate()
    {
        Debug.Log("Fixed estado Parado");
    }

    public void Exit()
    {
        Debug.Log("saiu do estado Parado");
    }
}
