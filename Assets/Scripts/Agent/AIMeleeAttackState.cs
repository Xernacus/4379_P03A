using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using UnityEngine.UI;

public class AIMeleeAttackState : AIState
{
    private NavMeshAgent _agent;
    private GameObject _target;
    //private PlayerController _playerController;
    private AISensor _sensor;
    private float _updateTimer;
    private float _scanTimer;
    private Vector3 _pastLocation1;
    public Animator animator;
    private bool _attacking = false;
    private float _speed;

    public void Enter(AIAgent agent)
    {
        animator = agent.gameObject.GetComponentInChildren<Animator>();
        _agent = agent.gameObject.GetComponent<NavMeshAgent>();
        _sensor = agent.gameObject.GetComponent<AISensor>();
        _target = GameObject.FindGameObjectWithTag("Player");
        _pastLocation1 = _target.transform.position;
        _sensor.distance = 4f;
        _sensor.angle = 90f;
        _speed = _agent.speed;
    }

    public void Exit(AIAgent agent)
    {

    }
    public AIStateID GetID()
    {
        return AIStateID.MeleeAttack;
    }

    public void Update(AIAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        _updateTimer -= Time.deltaTime;

        if (!_attacking && _updateTimer < 0)
        {
            _agent.destination = FindBehindPath(agent);
            _updateTimer = 0.25f;
        }

        _scanTimer -= Time.deltaTime;

        if (_scanTimer < 0)
        {
            _scanTimer = 0.5f;
            _sensor.Scan();
            GameObject[] player = _sensor.Filter(new GameObject[1], "Player");
            if (player[0] != null && _attacking != true)
            {
                Debug.Log("Attack");
                AttemptAttack();
                _attacking = true;
                
            }

        }
    }

    private Vector3 FindBehindPath(AIAgent agent)
    {
        Vector3 destination = _pastLocation1;
        //_pastLocation1 = _pastLocation2;
        _pastLocation1 = _target.transform.position;
        return destination;
    }

    private async void AttemptAttack()
    {
        await Attack();
        _attacking = false;
        _agent.speed = _speed;
        _updateTimer = 0.1f;
    }

    public async Task Attack()
    {
        _agent.speed = 0;

        await TelegraphFlash();

        _agent.destination = _target.transform.position;
        _agent.speed = _speed*3f;

        for (int i = 0; i < 10; i++)
        {
            Debug.Log(_agent.speed);
            await Task.Delay(100);
            _agent.speed = _agent.speed - (_agent.speed - _speed)*.1f;
        }
        
    }

    public async Task TelegraphFlash()
    {

    }
}
