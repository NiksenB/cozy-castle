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
    public bool wantsPat = true;
    public float speed = 4.0f;
    public float interactionRadius = 0.8f;
    public float visibilityRange = 5.0f;
    public Transform playerPosition;
    public Animator animator;
    private CreatureState currentState = CreatureState.Idle;
    private GameObject heartBubble;
    private bool isFrozen = false;
    private bool hasEyesOnPlayer = false;

    public enum FacingDirection { Up, Down, Left, Right }
    public FacingDirection facingDirection = FacingDirection.Down;

    public void GivePat()
    { 
        if (currentState == CreatureState.Idle || currentState == CreatureState.Moving)
        {
            wantsPat = false; 
            ChangeState(CreatureState.Interacting);
            Debug.Log(creatureName + " has been patted.");
        }
        else
        {
            Debug.Log(creatureName + " is not in a state to be patted.");
        }
    }

    public void SetAnimator(Animator anim)
    {
        animator = anim;
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the Creature object.");
        }
    }

    public void SetLoveBubble(GameObject loveBubble)
    {
        this.heartBubble = loveBubble;
        if (loveBubble != null) loveBubble.SetActive(false);
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
        if (playerPosition != null && IsAlert())
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerPosition.position);

            if (distanceToPlayer <= visibilityRange)
            {
                if (hasEyesOnPlayer) return true;

                Vector2 facingVector = Vector2.zero;
                switch (facingDirection)
                {
                    case FacingDirection.Up: facingVector = Vector2.up; break;
                    case FacingDirection.Down: facingVector = Vector2.down; break;
                    case FacingDirection.Left: facingVector = Vector2.left; break;
                    case FacingDirection.Right: facingVector = Vector2.right; break;
                }

                Vector2 toPlayer = (playerPosition.position - transform.position).normalized;
                return Vector2.Angle(facingVector, toPlayer) < 60.0f;
            }
        }

        if (playerPosition == null) Debug.LogWarning("Player position is not set. Cannot check visibility.");
        return false;
    }

    public void MoveTowardsPlayer(Rigidbody2D myRigidbody)
    {
        if (isFrozen)
        {
            Debug.Log(creatureName + " is frozen and cannot move.");
            return;
        }

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
            if (!IsPlayerInInteractionRange() && wantsPat)
            {
                if (!hasEyesOnPlayer)
                {
                    ChangeState(CreatureState.Reacting);
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

    public virtual bool IsAlert()
    {
        return currentState != CreatureState.Sleeping;
    }

    public void ChangeState(CreatureState newState)
    {
        if (isFrozen) return;

        if (currentState == newState) return;

        currentState = newState;
        Debug.Log(creatureName + " changed state to: " + currentState);

        if (heartBubble != null)
        {
            heartBubble.SetActive(
                newState == CreatureState.Reacting ||
                newState == CreatureState.Interacting
            );
        }

        switch (newState)
            {
                case CreatureState.Idle:
                    animator.SetBool("isMoving", false);
                    break;
                case CreatureState.Moving:
                    animator.SetBool("isMoving", true);
                    break;
                case CreatureState.Interacting:
                    hasEyesOnPlayer = true;
                    animator.SetTrigger("interact");
                    StartCoroutine(FreezeForSeconds(1.0f));
                    break;
                case CreatureState.Reacting:
                    hasEyesOnPlayer = true;
                    animator.SetTrigger("react");
                    StartCoroutine(FreezeForSeconds(0.6f));
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
    
    private System.Collections.IEnumerator FreezeForSeconds(float seconds)
    {
        isFrozen = true;
        yield return new WaitForSeconds(seconds);
        isFrozen = false;
        
        if (currentState == CreatureState.Reacting
            || currentState == CreatureState.Interacting)
            ChangeState(CreatureState.Idle);
    }
}
