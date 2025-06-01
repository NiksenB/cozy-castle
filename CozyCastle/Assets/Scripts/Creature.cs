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
                animator.SetBool("isMoving", true);
                Debug.Log(creatureName + " is now moving towards the player.");
            }

            // Update facing direction based on player position
            Vector2 directionToPlayer = (playerPosition.position - transform.position).normalized;
            if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
            {
                facingDirection = directionToPlayer.x > 0 ? FacingDirection.Right : FacingDirection.Left;
            }
            else
            {
                facingDirection = directionToPlayer.y > 0 ? FacingDirection.Up : FacingDirection.Down;
            }

            animator.SetFloat("horizontal", directionToPlayer.x);
            animator.SetFloat("vertical", directionToPlayer.y);

            Vector2 temp = Vector2.MoveTowards(transform.position, playerPosition.position, speed * Time.deltaTime);
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
            hasEyesOnPlayer = true;
        }
        else if (!isPlayerVisible && hasEyesOnPlayer)
        {
            Debug.Log(creatureName + " has lost sight of the player.");
            ChangeState(CreatureState.Idle);
            animator.SetBool("isMoving", false);
            hasEyesOnPlayer = false;
        }
    }

    public void ReactToPlayer(Rigidbody2D myRigidbody)
    {
        if (!hasEyesOnPlayer)
        {
            hasEyesOnPlayer = true;
            Debug.Log(creatureName + " has eyes on the player.");
            ChangeState(CreatureState.Reacting);
        }

        Debug.Log(creatureName + " is reacting to the player.");
        if (isFriendly)
        {
            if (!IsPlayerInInteractionRange())
            {
                MoveTowardsPlayer(myRigidbody);
            }
            else
            {
                ChangeState(CreatureState.Idle);
                animator.SetBool("isMoving", false);
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

        if (newState == CreatureState.Sleeping)
        {
            hasEyesOnPlayer = false;
        }

        currentState = newState;
        Debug.Log(creatureName + " changed state to: " + currentState);
    }
}
