using UnityEngine;

public abstract class State
{
    public float StateDuration { get; private set; } = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Enter()
    {
        StateDuration = 0;
    }

    public virtual void Exit()
    {

    }

    public virtual void FixedTick()
    {

    }

    public virtual void Tick() 
    { 
        StateDuration += Time.deltaTime;
    }
}
