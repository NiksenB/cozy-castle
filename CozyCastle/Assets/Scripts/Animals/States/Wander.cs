using UnityEngine;

public class Wander : AnimalState
{
    private float wanderDuration = 30.0f;
    private Rigidbody2D myRigidbody;
    private Transform myTransform;
    private Collider2D boundsCollider;
    private float speed = 1.5f;
    private Vector3 currentDirection;
    private int currentDirectionInt = 0;
    private float wanderPauseDuration = 5.0f;
    private float wanderPauseStartTime;
    private bool directionNeedsChange = false; 

    public Wander(Animal animal, Animator animator, Rigidbody2D myRigidbody, Transform myTransform, float speed) : base(animal, animator)
    {
        this.myRigidbody = myRigidbody;
        this.myTransform = myTransform;
        this.speed = speed;
        boundsCollider = myTransform.parent.GetComponent<BoxCollider2D>();
        if (boundsCollider == null)
        {
            Debug.LogError("Bounds collider not found on animal: " + animal.name);
        }
    }

    public override void EnterState()
    {
        base.EnterState();
        animator.SetBool("isMoving", true);
        wanderDuration = Random.Range(5.0f, 30.0f); // Ideal 5-30
        currentDirection = animal.GetDirection();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (isExitingState) return;
        if (!(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")))
        {
            return; // Wait until other animation is finished
        }
        if (Time.time > startTime + wanderDuration)
        {
            ExitState();
            return;
        }
        if (Time.time > wanderPauseStartTime)
        {
            if (Time.time < wanderPauseStartTime + wanderPauseDuration)
            {
                animator.SetBool("isMoving", false);
                return;
            }
        }
        animator.SetBool("isMoving", true);

        if (directionNeedsChange || Random.Range(0, 1000) < 5)
        {
            ChangeDirection();
            directionNeedsChange = false;
        }
        Move();
    }

    private void Move()
    {
        Vector3 targetPosition = myTransform.position + speed * Time.deltaTime * currentDirection;

        Vector3 min = boundsCollider.bounds.min;
        Vector3 max = boundsCollider.bounds.max;
        if (targetPosition.x >= min.x
            && targetPosition.x <= max.x
            && targetPosition.y >= min.y
            && targetPosition.y <= max.y)
        {
            myRigidbody.MovePosition(targetPosition);
        }
        else
        {
            ChangeDirection();
            wanderPauseDuration = Random.Range(1.0f, 5.0f); // Ideal 1-5
            wanderPauseStartTime = Time.time + Random.Range(3.0f, 7.0f); // Ideal 3-7
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        animator.SetBool("isMoving", false);
    }

    void ChangeDirection()
    {
        int d = Random.Range(0, 4);
        if (d == currentDirectionInt)
        {
            d = (d + 1) % 4;
        }
        Vector2 temp = Vector2.zero;
        switch (d)
        {
            case 0: temp = Vector2.down; break;
            case 1: temp = Vector2.left; break;
            case 2: temp = Vector2.up; break;
            case 3: temp = Vector2.right; break;
        }
        animal.UpdateDirection(temp);
        animal.ChangeAnim(temp);
        currentDirection = temp;
        currentDirectionInt = d;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        wanderPauseStartTime = Time.time;
        directionNeedsChange = true;
    }

    public override void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time > wanderPauseStartTime + wanderPauseDuration + 1.0f)
        {
            directionNeedsChange = true;
        }
    }
}
