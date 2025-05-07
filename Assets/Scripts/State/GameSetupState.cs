using UnityEngine;

public class GameSetupState : State
{
    private GameFSM _stateMachine;
    private GameController _controller;
    private PlayerController _player;
    public GameSetupState(GameFSM stateMachine, GameController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("State: Game Setup");
        _player = _controller.UnitSpawner.Spawn(_controller.PlayerUnitPrefab, _controller.PlayerUnitSpawnLocation).GetComponent<PlayerController>();
        _player.GameController = _controller;
        SaveManager.Instance.Load();
    }

    public override void Exit()
    {
        base.Exit();
        _player.SubscribeToHealthEvent();
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public override void Tick()
    {
        base.Tick();

        _stateMachine.ChangeState(_stateMachine.GamePlayState);
    }
}
