using System.Drawing;
using UnityEngine;

public class NPCPatrol : NPCState
{
    private float timeUntilPause = 30.0f;
    private Rigidbody2D myRigidbody;
    private Transform myTransform;
    private EdgeCollider2D pathCollider;
    private float speed = 1.5f;
    private int pointsInPath;
    private int pointIndex = 0;

    public NPCPatrol(NPC npc, Animator animator, Rigidbody2D myRigidbody, Transform myTransform, float speed, EdgeCollider2D pathCollider) : base(npc, animator)
    {
        this.myRigidbody = myRigidbody;
        this.myTransform = myTransform;
        this.speed = speed;
        this.pathCollider = pathCollider;
        pointsInPath = pathCollider.pointCount;
        if (pointsInPath < 2)
        {
            Debug.LogError("NPC " + npc.name + " has an invalid patrol path. It must have at least 2 points.");
        }
    }

    public override void EnterState()
    {
        base.EnterState();
        SetStartPoint();
        animator.SetBool("isMoving", true);
        timeUntilPause = Random.Range(10.0f, 60.0f);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (isExitingState) return;
        if (Time.time > startTime + timeUntilPause)
        {
            ExitState();
            return;
        }

        if (npc.IsPlayerThisClose(1.5f))
        {
            animator.SetBool("isMoving", false);
            return;
        }

        animator.SetBool("isMoving", true);
        Move();
    }

    private void SetStartPoint()
    {
        float closestDist = float.MaxValue;
        for (int i = 0; i < pointsInPath; i++)
        {
            float dist = Vector2.SqrMagnitude(pathCollider.points[i] - (Vector2)myTransform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                pointIndex = i;
            }
        }
    }

    private void Move()
    {
        Vector2 dis = pathCollider.points[pointIndex] - (Vector2)myTransform.position;
        Vector2 direction = dis.normalized;
        if (dis.sqrMagnitude < 0.1f * 0.1f)
        {
            int nextIndex = (pointIndex + 1) % pointsInPath;
            pointIndex = nextIndex;
            direction = (pathCollider.points[pointIndex] - (Vector2)myTransform.position).normalized;
        }
        Vector3 targetPosition = myTransform.position + speed * Time.deltaTime * (Vector3)direction;
        myRigidbody.MovePosition(targetPosition);
        npc.UpdateDirection(direction);
        npc.ChangeAnim(direction);
    }

    public override void ExitState()
    {
        base.ExitState();
        animator.SetBool("isMoving", false);
    }
}
