using UnityEngine;

public class GameEndState : State
{
    private GameFSM _stateMachine;
    private GameController _controller;

    public GameEndState(GameFSM stateMachine, GameController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
        Enemy[] enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            GameObject.Destroy(enemy.gameObject);
        }
        Debug.Log("State: Game End");

        SaveManager.Instance.Save();
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
    }
}
