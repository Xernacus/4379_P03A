using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AIStateID { 
    EnemyWander,
    MeleeAttack,
    EnemyWindow
}

public interface AIState
{
    AIStateID GetID();
    void Enter(AIAgent agent);
    void Update(AIAgent agent);
    void Exit(AIAgent agent);
}
