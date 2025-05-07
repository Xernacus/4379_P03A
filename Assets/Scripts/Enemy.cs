using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Health _health;
    [SerializeField]
    private AIAgent _agent;
    [SerializeField]
    private NavMeshAgent _navAgent;

    [SerializeField]
    private SpriteRenderer _sprite;

    private Animator _animator;

    private void Start()
    {
        _animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(1);
        }
    }

    private void Update()
    {
        
        if (_navAgent.desiredVelocity.x > 0 && _agent.stateMachine.currentState != AIStateID.EnemyStunned)
        {
            _sprite.flipX = true;
        }
        else if (_navAgent.desiredVelocity.x < 0 && _agent.stateMachine.currentState != AIStateID.EnemyStunned)
        {
            _sprite.flipX = false;
        }
    }

    public void Damage(PlayerController player)
    {
        _agent.ChangeState(AIStateID.EnemyStunned);
        Vector3 temp = (gameObject.transform.position + (gameObject.transform.position - player.gameObject.transform.position).normalized * 10f);
        _navAgent.destination = new Vector3(temp.x, gameObject.transform.position.y, temp.z);
        
        _health.TakeDamage(1);
    }

    public void SetSpriteFlashing(bool flashing)
    {
        if (flashing)
        {
            _sprite.color = Color.red;
        }
        else
        {
            _sprite.color = Color.white;
        }      
    }

    public void PlayAttackAnim()
    {
        _animator.Play("DogAnim", 0, 0.1f);
    }
}
