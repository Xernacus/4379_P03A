using UnityEngine;

public class GamePlayState : State
{
    private GameFSM _stateMachine;
    private GameController _controller;

    private float _enemySpawnTime = 15;
    private float _enemySpawnTimeIncrement = 15;

    public GamePlayState(GameFSM stateMachine, GameController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("State: Game Play");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public override void Tick()
    {
        base.Tick();
        if (_controller.ElapsedTime >= _enemySpawnTime)
        {
            SpawnEnemy();
            _enemySpawnTime += _enemySpawnTimeIncrement;
        }
    }

    private void SpawnEnemy()
    {
        UnitSpawner spawner = _controller.EnemySpawners[Random.Range(0, _controller.EnemySpawners.Length)];
        spawner.Spawn(_controller.EnemyUnitPrefab, spawner.gameObject.transform);
    }
}
