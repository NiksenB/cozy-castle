using Unity.VisualScripting;
using UnityEngine;

public class NPCFollow : NPCState
{
    private Transform target;
    private Transform transform;
    private Rigidbody2D rigidbody;
    private GameObject exclamationBubble;
    private readonly float tooClose = 1.5f;
    private readonly float tooFar = 2f;

    public NPCFollow(NPC npc, Animator animator, GameObject exclamationBubble, Transform transform, Rigidbody2D rigidbody, Transform target, float giveUpDistance) : base(npc, animator)
    {
        this.exclamationBubble = exclamationBubble;
        this.transform = transform;
        this.rigidbody = rigidbody;
        this.target = target;
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
            Debug.Log(npc.name + " is waiting for Reaction animation to finish.");
            return; // Wait until the "Reaction" animation is finished
        }
        
        float distance = DistanceToTarget();

        switch (distance)
        {
            case float d when d < tooClose:
                animator.SetBool("isMoving", false);
                npc.FaceTarget(target);
                break;
            case float d when d < tooFar && animator.GetBool("isMoving") == false:
                npc.FaceTarget(target);
                break;
            default:
                animator.SetBool("isMoving", true);
                FollowTarget(distance);
                break;
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        animator.SetBool("isMoving", false);
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
            Vector2 temp = Vector2.MoveTowards(transform.position, target.position, 0.7f * (3f + distance) * Time.deltaTime);

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