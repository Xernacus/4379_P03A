using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;

public class AIEnemyEnterWindowState : AIState
{
    private NavMeshAgent _agent;
    private Enemy _enemy;
    private GameObject _target;
    private GameObject[] _windows;
    private Window _window;
    private bool _attacking = false;

    public void Enter(AIAgent agent)
    {
        _agent = agent.gameObject.GetComponent<NavMeshAgent>();
        _windows = GameObject.FindGameObjectsWithTag("Window");
        _window = GetClosestWindow();

        if (_window.Barricaded)
        {
            _agent.destination = _window.OutsidePoint.transform.position;
        }
        else
        {
            _agent.destination = _window.VaultPoint.transform.position;
        }

        _enemy = agent.gameObject.GetComponent<Enemy>();
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
            if (!_window.Barricaded && !_attacking)
            {
                agent.ChangeState(AIStateID.EnemyWander);
            }
            else if(!_attacking)
            {
                AttemptAttack();
            }
        }
    }

    private Window GetClosestWindow()
    {
        float closest = 9999;
        Window window = null;
        for (int i = 0; i < _windows.Length; i++)
        {
            if ((_windows[i].transform.position - _agent.gameObject.transform.position).magnitude < closest)
            {
                closest = (_windows[i].transform.position - _agent.gameObject.transform.position).magnitude;
                window = _windows[i].GetComponent<Window>();
            }
        }
        return window;
    }

    private async void AttemptAttack()
    {
        Debug.Log("Attempting attaack");
        _attacking = true;
        await Attack();
        await Task.Delay(1000);
        _attacking = false;
        if (!_window.Barricaded)
        {
            _agent.destination = _window.VaultPoint.transform.position;
        }
    }

    public async Task Attack()
    {
        _enemy.PlayAttackAnim();
        await TelegraphFlash();
        _window.Damage();
    }

    public async Task TelegraphFlash()
    {
        _enemy.SetSpriteFlashing(true);
        await Task.Delay(200);
        _enemy.SetSpriteFlashing(false);
    }

}
