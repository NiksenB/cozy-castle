using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class Chase : AnimalState
{
    private Transform targetPlayer;
    private Transform transform;
    private Rigidbody2D rigidbody;
    private float speed;
    private GameObject heartBubble;

    public Chase(Animal animal, Animator animator, GameObject heartBubble, Transform transform, Rigidbody2D rigidbody, float speed) : base(animal, animator)
    {
        this.heartBubble = heartBubble;
        this.transform = transform;
        this.rigidbody = rigidbody;
        this.speed = speed;
    }

    public override void EnterState()
    {
        base.EnterState();
        animator.SetBool("isMoving", false);
        heartBubble.SetActive(true);
        animator.SetTrigger("react");
    }

    public override void UpdateState()
    {
        if (isExitingState) return;
        base.UpdateState();
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Reaction"))
        {
            return; // Wait until the "Reaction" animation is finished
        }
        animator.SetBool("isMoving", true);
        heartBubble.SetActive(false);
        MoveTowardsPlayer();
    }

    public override void ExitState()
    {
        base.ExitState();
        heartBubble.SetActive(false);
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
