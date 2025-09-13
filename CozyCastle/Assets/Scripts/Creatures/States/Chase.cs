using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class Chase : AnimalState
{
    private Transform targetPlayer;
    private Transform transform;
    private Rigidbody2D rigidbody;
    private float speed;

    public Chase(Animal animal, Animator animator, Transform transform, Rigidbody2D rigidbody, float speed) : base(animal, animator)
    {
        this.transform = transform;
        this.rigidbody = rigidbody;
        this.speed = speed;
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log(animal.name + " has entered Chase state!!!");
        animator.SetBool("isMoving", true);
    }

    public override void UpdateState()
    {
        Debug.Log(animal.name + " is in Chase state...");
        if (isExitingState) return;
        base.UpdateState();
        MoveTowardsPlayer();
    }

    public override void ExitState()
    {
        base.ExitState();
        animator.SetBool("isMoving", false);
    }

    public void SetTargetPlayer(Transform targetPlayer)
    {
        this.targetPlayer = targetPlayer;
    }

    public void MoveTowardsPlayer()
    {
        if (targetPlayer != null)
        {
            Debug.Log(animal.name + " is moving towards player at position: " + targetPlayer.position);
            Vector2 temp = Vector2.MoveTowards(transform.position, targetPlayer.position, speed * Time.deltaTime);

            animal.UpdateDirection(temp - (Vector2)transform.position);
            animal.ChangeAnim(temp - (Vector2)transform.position);
            rigidbody.MovePosition(temp);
        }
        else
        {
            Debug.Log("Player position is not set. Cannot move towards player.");
        }
    }
}
