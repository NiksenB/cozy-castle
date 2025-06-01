using UnityEngine;

public enum CreatureState
{
    Idle,
    Moving,
    Interacting,
    Reacting,
    Sleeping,
}

public class Creature : MonoBehaviour
{
    public string creatureName = "Default Creature";
    public bool isFriendly = true;
    public bool hasEyesOnPlayer = false;
    public CreatureState currentState = CreatureState.Idle;
    public float speed = 4.0f;
    public float interactionRadius = 0.8f;
    public float visibilityRange = 5.0f;
    public Transform playerPosition;
    public Animator animator;

    public enum FacingDirection { Up, Down, Left, Right }
    public FacingDirection facingDirection = FacingDirection.Down;

    public void SetAnimator(Animator anim)
    {
        animator = anim;
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the Creature object.");
        }
    }

    public bool IsPlayerInInteractionRange()
    {
        if (playerPosition != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);
            return distanceToPlayer <= interactionRadius;
        }
        return false;
    }

    public bool IsPlayerVisible()
    {
        if (playerPosition != null && currentState != CreatureState.Sleeping)
        {
            Vector2 toPlayer = (playerPosition.position - transform.position).normalized;

            Vector2 facingVector = Vector2.zero;
            switch (facingDirection)
            {
                case FacingDirection.Up: facingVector = Vector2.up; break;
                case FacingDirection.Down: facingVector = Vector2.down; break;
                case FacingDirection.Left: facingVector = Vector2.left; break;
                case FacingDirection.Right: facingVector = Vector2.right; break;
            }
            float angle = Vector2.Angle(facingVector, toPlayer);

            float distanceToPlayer = Vector2.Distance(transform.position, playerPosition.position);

            return
                distanceToPlayer <= visibilityRange &&
                angle < 60.0f;
        }
        Debug.LogWarning("Player position is not set. Cannot check visibility.");
        return false;
    }

    public void MoveTowardsPlayer(Rigidbody2D myRigidbody)
    {
        if (playerPosition != null)
        {
            if (currentState != CreatureState.Moving)
            {
                ChangeState(CreatureState.Moving);
            }

            Vector2 temp = Vector2.MoveTowards(transform.position, playerPosition.position, speed * Time.deltaTime);

            UpdateDirection(temp - (Vector2)transform.position);
            ChangeAnim(temp - (Vector2)transform.position);
            myRigidbody.MovePosition(temp);
        }
        else
        {
            Debug.Log("Player position is not set. Cannot move towards player.");
        }
    }

    public void UpdatePlayerVisibility(bool isPlayerVisible = true)
    {
        if (isPlayerVisible && !hasEyesOnPlayer)
        {
            Debug.Log(creatureName + " has spotted the player.");
            ChangeState(CreatureState.Reacting);
        }
        else if (!isPlayerVisible && hasEyesOnPlayer)
        {
            Debug.Log(creatureName + " has lost sight of the player.");
            hasEyesOnPlayer = false;
            ChangeState(CreatureState.Idle);
        }
    }

    public void ReactToPlayer(Rigidbody2D myRigidbody)
    {
        if (isFriendly)
        {
            if (!IsPlayerInInteractionRange())
            {
                if (!hasEyesOnPlayer)
                {
                    ChangeState(CreatureState.Reacting);;
                    hasEyesOnPlayer = true;
                }
                
                MoveTowardsPlayer(myRigidbody);
            }
            else
            {
                ChangeState(CreatureState.Idle);
            }
        }
        else
        {
            Debug.Log(creatureName + " is not friendly and does not approach the player.");
        }
    }

    public void ChangeState(CreatureState newState)
    {
        if (currentState == newState)
        {
            return;
        }

        currentState = newState;
        Debug.Log(creatureName + " changed state to: " + currentState);

        switch (newState)
        {
            case CreatureState.Idle:
                animator.SetBool("isMoving", false);
                break;
            case CreatureState.Moving:
                animator.SetBool("isMoving", true);
                break;
            case CreatureState.Interacting:
                // animator.SetTrigger("interact");
                animator.SetBool("isMoving", false);
                break;
            case CreatureState.Reacting:
                // animator.SetTrigger("react");
                hasEyesOnPlayer = true;
                animator.SetBool("isMoving", false);
                break;
            case CreatureState.Sleeping:
                animator.SetBool("isSleeping", true);
                hasEyesOnPlayer = false;
                break;
        }
    }

    private void ChangeAnim(Vector2 direction)
    {
        if (animator == null) return;

        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);
    }

    private void UpdateDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
                facingDirection = FacingDirection.Right;
            else
                facingDirection = FacingDirection.Left;
        }
        else
        {
            if (direction.y > 0)
                facingDirection = FacingDirection.Up;
            else
                facingDirection = FacingDirection.Down;
        }
    }
}
