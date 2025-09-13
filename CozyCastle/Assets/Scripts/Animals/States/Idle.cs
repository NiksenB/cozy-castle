using UnityEngine;

public class Idle : AnimalState
{
    private float idleDuration = 2.0f;

    public Idle(Animal animal, Animator animator) : base(animal, animator)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        animator.SetBool("isMoving", false);
        idleDuration = Random.Range(3.0f, 10.0f);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (startTime < Time.time - idleDuration)
        {
            ExitState();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}