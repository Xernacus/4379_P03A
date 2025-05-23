using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using System;

public class GameController : MonoBehaviour
{
    [field: SerializeField]
    public Unit PlayerUnitPrefab { get; private set; }
    [field: SerializeField]
    public Transform PlayerUnitSpawnLocation { get; private set; }
    [field: SerializeField]
    public UnitSpawner UnitSpawner { get; private set; }
    [field: SerializeField]
    public Unit EnemyUnitPrefab { get; private set; }
    [field: SerializeField]
    public UnitSpawner[] EnemySpawners { get; private set; }
    [field: SerializeField]
    public GameHUDController GameHUDController { get; private set; }
    [field: SerializeField]
    public CinemachineCamera Camera { get; private set; }
    [field: SerializeField]
    public GameFSM FSM { get; private set; }

    [SerializeField]
    private AudioClip _sfx;

    [SerializeField] private float _timeToWin = 300;
    public float ElapsedTime { get; private set; }
    public bool HasWon { get; private set; }

    public event Action OnWin = delegate { };
    public event Action OnLose = delegate { };

    
private void Start()
    {

        ElapsedTime = 0;
        HasWon = false;
    }

    private void Update()
    {
        ElapsedTime += Time.deltaTime;

        if (ElapsedTime >= _timeToWin && !HasWon)
        {
            EnterWinState();
        }
        
    }

    public void EnterWinState()
    {
        HasWon = true;
        OnWin?.Invoke();
        FSM.ChangeState(FSM.GameEndState);
    }

    public void EnterLoseState()
    {
        OnLose?.Invoke();
        FSM.ChangeState(FSM.GameEndState);
    }

    public void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void PlayMusic()
    {
        if (_sfx != null)
        {
            AudioHelper.PlayClip2D(_sfx, .4f);
        }
    }
}
