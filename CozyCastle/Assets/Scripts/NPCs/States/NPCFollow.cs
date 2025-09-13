using Unity.VisualScripting;
using UnityEngine;

public class NPCFollow : NPCState
{
    private Transform target;
    private Transform transform;
    private Rigidbody2D rigidbody;
    GameObject exclamationBubble;
    private float minDistance = 3f;
    private float maxDistance = 5f;

    public NPCFollow(NPC npc, Animator animator, GameObject exclamationBubble, Transform transform, Rigidbody2D rigidbody) : base(npc, animator)
    {
        this.exclamationBubble = exclamationBubble;
        this.transform = transform;
        this.rigidbody = rigidbody;
    }

    public override void EnterState()
    {
        base.EnterState();
        exclamationBubble.SetActive(false);
    }

    public override void UpdateState()
    {
        if (isExitingState) return;
        base.UpdateState();
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Reaction"))
        {
            return; // Wait until the "Reaction" animation is finished
        }

        float distance = DistanceToTarget();
        if (distance > maxDistance)
        {
            animator.SetBool("isMoving", true);
            FollowTarget(distance);
        }
        else if (distance < minDistance)
        {
            animator.SetBool("isMoving", false);
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        animator.SetBool("isMoving", false);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private float DistanceToTarget()
    {
        if (target != null)
        {
            return Vector2.Distance(transform.position, target.position);
        }
        else
        {
            Debug.Log("Target position is not set. Cannot calculate distance.");
            return Mathf.Infinity;
        }
    }

    public void FollowTarget(float distance)
    {
        if (target != null)
        {
            Vector2 temp = Vector2.MoveTowards(transform.position, target.position, 0.3f * distance * Time.deltaTime);

            npc.UpdateDirection(temp - (Vector2)transform.position);
            npc.ChangeAnim(temp - (Vector2)transform.position);
            rigidbody.MovePosition(temp);
        }
        else
        {
            Debug.Log("Target position is not set. Cannot move towards target.");
        }
    }
}