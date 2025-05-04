using UnityEngine;

[RequireComponent(typeof(GameController))]
public class GameFSM : StateMachineMB
{
    private GameController _controller;

    public GameSetupState SetupState { get; private set; }
    public GamePlayState GamePlayState { get; private set; }
    public GameEndState GameEndState { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _controller = gameObject.GetComponent<GameController>();
        SetupState = new GameSetupState(this, _controller);
        GamePlayState = new GamePlayState(this, _controller);
        GameEndState = new GameEndState(this, _controller);
    }

    private void Start()
    {
        ChangeState(SetupState);
    }
}
