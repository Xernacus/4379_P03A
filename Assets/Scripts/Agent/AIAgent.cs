using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateID initialState;
    public AIAgentConfig config;

   
    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIEnemyEnterWindowState());
        stateMachine.RegisterState(new AIEnemyWanderState());
        stateMachine.RegisterState(new AIMeleeAttackState());
        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

 
    public void ChangeState(AIStateID newState)
    {
        stateMachine.ChangeState(newState);
    }
    
}
