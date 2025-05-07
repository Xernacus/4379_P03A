using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using UnityEngine.UI;

public class AIStunnedState : AIState
{
    private NavMeshAgent _agent;
    private float _updateTimer;
    private float _speed = 3.5f;
    private Enemy _enemy;
    
        
    public void Enter(AIAgent agent)
    {
        _agent = agent.gameObject.GetComponent<NavMeshAgent>();
        _agent.speed = _speed * 100f;
        _updateTimer = .5f;
        _enemy = agent.gameObject.GetComponent<Enemy>();
        _enemy.SetSpriteFlashing(true);
        //Debug.Log(_agent.destination) ;
    }

    public void Exit(AIAgent agent)
    {
        
    }
    public AIStateID GetID()
    {
        return AIStateID.EnemyStunned;
    }

    public void Update(AIAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        _updateTimer -= Time.deltaTime;

        _agent.speed = _agent.speed - (_agent.speed) * Time.deltaTime;

        if (_updateTimer < 0)
        {
            Debug.Log("Exit Stunned");
            _agent.speed = _speed;
            agent.ChangeState(AIStateID.MeleeAttack);
        }
        else if(_updateTimer < .25f)
        {
            _agent.speed = _speed;
            _enemy.SetSpriteFlashing(false);
        }
    }
}
