using UnityEngine;

public enum CreatureState
{
    Idle,
    Moving,
    Interacting,
    Reacting,
    Sleeping,
}

public class Creature : VisionAI, IInteractable
{
    [SerializeField] protected string creatureName = "Default Creature";
    [SerializeField] protected bool isFriendly = true;
    [SerializeField] protected bool wantsPat = true;
    [SerializeField] protected float speed = 4.0f;
    protected Animator animator;
    protected CreatureState currentState = CreatureState.Idle;
    protected GameObject heartBubble;
    protected bool isFrozen = false;
    protected Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponentInChildren<Rigidbody2D>();
        if (myRigidbody == null) Debug.LogError("Rigidbody2D component not found on the Bunny object.");

        animator = GetComponentInChildren<Animator>();
        if (animator == null) Debug.LogError("Animator component not found on the Bunny object.");
        else SetAnimator(animator);

        if (transform.Find("LoveBubble") != null)
        {
            heartBubble = transform.Find("LoveBubble").gameObject;
            heartBubble.SetActive(false);
        }
        else
        {
            Debug.LogError("Heart Bubble prefab is not assigned. Please assign it in the inspector.");
        }
    }

    public void Interact(GameObject player)
    {
        GetPat(player);
    }

    public void GetPat(GameObject player)
    {
        if (currentState == CreatureState.Idle || currentState == CreatureState.Moving)
        {
            wantsPat = false; 
            ChangeState(CreatureState.Interacting);
            Debug.Log(creatureName + " has been patted by " + player.name + ".");
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
    public void MoveTowardsPlayer()
    {
        if (isFrozen)
        {
            return;
        }

        if (targetPlayer != null)
        {
            if (currentState != CreatureState.Moving)
            {
                ChangeState(CreatureState.Moving);
            }

            Vector2 temp = Vector2.MoveTowards(transform.position, targetPlayer.transform.position, speed * Time.deltaTime);

            UpdateDirection(temp - (Vector2)transform.position);
            ChangeAnim(temp - (Vector2)transform.position);
            myRigidbody.MovePosition(temp);
        }
        else
        {
            Debug.Log("Player position is not set. Cannot move towards player.");
        }
    }

    protected override void DiscoverPlayer()
    {
        ChangeState(CreatureState.Reacting);
    }

    protected override void LoseSight()
    {
        ChangeState(CreatureState.Idle);
    }

    protected override void PlayerInVisibilityRange()
    {
        if (isFriendly && wantsPat)
        {
            MoveTowardsPlayer();
        }
    }

    protected override bool IsAlert()
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
