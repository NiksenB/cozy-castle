using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Animal : VisionAI, IInteractable
{
    [SerializeField] protected string creatureName = "Default Creature";
    [SerializeField] protected bool isFriendly = true;
    [SerializeField] protected bool wantsPat = true;
    [SerializeField] protected float speed = 4.0f;
    protected Animator animator;
    protected GameObject heartBubble;
    protected Rigidbody2D myRigidbody;

    // Behavior toggles
    [SerializeField] protected bool canIdle = true;
    [SerializeField] protected bool canWander = true;
    [SerializeField] protected bool canChase = true;
    [SerializeField] protected bool canFlee = false;
    [SerializeField] protected bool canSleep = false;
    [SerializeField] protected bool canBeInteracted = true;
    private Idle idleBehavior;
    private Wander wanderBehavior;
    private Chase chaseBehavior;
    private Flee fleeBehavior;
    private Sleep sleepBehavior;
    private Interact interactBehavior;
    private List<AnimalState> randomBehaviors = new List<AnimalState>();
    private AnimalState currentBehavior;

    void Start()
    {
        myRigidbody = GetComponentInChildren<Rigidbody2D>();
        if (myRigidbody == null) Debug.LogError("Rigidbody2D component not found on the object.");

        animator = GetComponentInChildren<Animator>();
        if (animator == null) Debug.LogError("Animator component not found on the object.");

        if (transform.Find("LoveBubble") != null)
        {
            heartBubble = transform.Find("LoveBubble").gameObject;
            heartBubble.SetActive(false);
        }
        else
        {
            Debug.LogError("Heart Bubble prefab is not assigned. Please assign it in the inspector.");
        }

        if (canWander)
            wanderBehavior = new Wander(this, animator);
            randomBehaviors.Add(wanderBehavior);
        if (canChase)
            chaseBehavior = new Chase(this, animator, transform, myRigidbody, speed);
        if (canFlee)
            fleeBehavior = new Flee(this, animator, transform, myRigidbody, speed);
        if (canSleep)
            sleepBehavior = new Sleep(this, animator);
            randomBehaviors.Add(sleepBehavior);
            currentBehavior = sleepBehavior;
        if (canBeInteracted)
            interactBehavior = new Interact(this, animator, heartBubble);
        if (canIdle)
            idleBehavior = new Idle(this, animator);
            randomBehaviors.Add(idleBehavior);
            currentBehavior = idleBehavior;
    }

    void FixedUpdate()
    {
        if (currentBehavior != null && !currentBehavior.isExitingState)
            currentBehavior.UpdateState();
        else
        {
            Debug.LogWarning("Current behavior is not set. Setting to random behavior.");
            if (randomBehaviors.Count > 0)
            {
                ChangeBehavior(randomBehaviors[Random.Range(0, randomBehaviors.Count)]);
            }
            else { Debug.LogError("No behaviors available to switch to.");}
        }
    }

    public void Interact(GameObject player)
    {
        if (canBeInteracted)
        {
            wantsPat = false;
            ChangeBehavior(interactBehavior);
        }
    }

    protected override void DiscoverPlayer()
    {
        if (canChase || canFlee)
        {
            if (isFriendly && wantsPat && canChase)
            {
                animator.SetTrigger("react");
                Debug.Log(creatureName + " wants to be patted!");
                if (canBeInteracted) ChangeBehavior(interactBehavior);

                chaseBehavior.SetTargetPlayer(targetPlayer);
                ChangeBehavior(chaseBehavior);
                Debug.Log(creatureName + " has discovered the player and is now chasing.");
            }
            else if (!isFriendly && canFlee)
            {
                ChangeBehavior(fleeBehavior);
            }
        }
    }

    protected override void LoseSight()
    {
        if (canIdle)
        {
            Debug.Log(creatureName + " has lost sight of the player and is now idling.");
            ChangeBehavior(idleBehavior);
        }
    }

    protected override void PlayerInVisibilityRange()
    {
        if (canChase && isFriendly && wantsPat)
        {
            Debug.Log(creatureName + " is chasing.");
            chaseBehavior.SetTargetPlayer(targetPlayer);
            ChangeBehavior(chaseBehavior);
        }
        else if (canFlee && !isFriendly)
        {
            ChangeBehavior(fleeBehavior);
        }
    }

    protected override bool IsAlert()
    {
        return currentBehavior != sleepBehavior;
    }

    public void ChangeBehavior(AnimalState newBehavior)
    {
        Debug.Log(creatureName + " attempting to change behavior from " + (currentBehavior != null ? currentBehavior.GetType().Name : "null") + " to: " + (newBehavior != null ? newBehavior.GetType().Name : "null"));
        if (currentBehavior == newBehavior) return;

        currentBehavior?.ExitState();
        newBehavior?.EnterState();
        currentBehavior = newBehavior;
        Debug.Log(creatureName + " changed behavior to: " + currentBehavior?.GetType().Name);

        
    }

    public void ChangeAnim(Vector2 direction)
    {
        if (animator == null) return;

        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);
    }

    public void UpdateDirection(Vector2 direction)
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
