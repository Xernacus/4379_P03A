using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemyEnterWindowState : AIState
{
    private NavMeshAgent _agent;
    private GameObject _target;
    private GameObject[] _windows;

    public void Enter(AIAgent agent)
    {
        _agent = agent.gameObject.GetComponent<NavMeshAgent>();
        _windows = GameObject.FindGameObjectsWithTag("Window");
        GetClosestWindow();
    }

    public void Exit(AIAgent agent)
    {

    }
    public AIStateID GetID()
    {
        return AIStateID.EnemyWindow;
    }

    public void Update(AIAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            agent.ChangeState(AIStateID.EnemyWander);
        }
    }

    private void GetClosestWindow()
    {
        float closest = 9999;
        for (int i = 0; i < _windows.Length; i++)
        {
            if ((_windows[i].transform.position - _agent.gameObject.transform.position).magnitude < closest)
            {
                closest = (_windows[i].transform.position - _agent.gameObject.transform.position).magnitude;
                _agent.destination = _windows[i].transform.position;
            }
        }
    }
        

}
