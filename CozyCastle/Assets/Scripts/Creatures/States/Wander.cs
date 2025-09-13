using UnityEngine;

public class Wander : AnimalState
{
    public Wander(Animal animal, Animator animator) : base(animal, animator)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        animator.SetBool("isMoving", true);
        // Additional logic for entering the Sleep state can be added here
    }

    public override void UpdateState()
    {
        base.UpdateState();
        // Logic for the Sleep state can be added here
    }

    public override void ExitState()
    {
        base.ExitState();
        animator.SetBool("isMoving", false);
        // Additional logic for exiting the Sleep state can be added here
    }
}
