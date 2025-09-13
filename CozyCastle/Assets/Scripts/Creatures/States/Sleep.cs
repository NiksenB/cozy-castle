using UnityEngine;

public class Sleep : AnimalState
{
    private float sleepDuration = 5.0f; 
    public Sleep(Animal animal, Animator animator) : base(animal, animator)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        animator.SetBool("isMoving", false);
        animator.SetBool("isSleeping", true);
        sleepDuration = Random.Range(5.0f, 120.0f); 
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (startTime < Time.time - sleepDuration)
        {
            ExitState();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        animator.SetBool("isSleeping", false);
    }
}
