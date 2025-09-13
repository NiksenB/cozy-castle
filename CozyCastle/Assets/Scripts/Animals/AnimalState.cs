using NUnit.Framework;
using UnityEngine;

public class AnimalState
{
    protected Animal animal;
    protected Animator animator;
    protected float startTime = 0f;
    public bool isExitingState = false;

    public AnimalState(Animal animal, Animator animator)
    {
        this.animal = animal;
        this.animator = animator;
    }

    public virtual void EnterState()
    {
        isExitingState = false;
        startTime = Time.time;
    }

    public virtual void UpdateState() { if (isExitingState) return; }

    public virtual void ExitState()
    {
        isExitingState = true;
        startTime = 0f;
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("AnimalState collided with " + collision.gameObject.name);
    }

    public virtual void OnCollisionStay2D(Collision2D collision){}
}
