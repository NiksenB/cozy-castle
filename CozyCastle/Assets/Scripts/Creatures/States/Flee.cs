using NUnit.Framework;
using UnityEngine;

public class Flee : AnimalState
{
    private Transform targetPlayer;
    private Transform transform;
    private Rigidbody2D rigidbody;
    private float speed;
    public Flee(Animal animal, Animator animator, Transform transform, Rigidbody2D rigidbody, float speed) : base(animal, animator)
    {
        this.transform = transform;
        this.rigidbody = rigidbody;
        this.speed = speed;
    }

    public override void EnterState()
    {
        base.EnterState();
        animator.SetBool("isMoving", true);
        // Additional logic for entering the Flee state can be added here
    }

    public override void UpdateState()
    {
        base.UpdateState();
        // Logic for the Flee state can be added here
    }

    public override void ExitState()
    {
        base.ExitState();
        animator.SetBool("isMoving", false);
        // Additional logic for exiting the Flee state can be added here
    }
}