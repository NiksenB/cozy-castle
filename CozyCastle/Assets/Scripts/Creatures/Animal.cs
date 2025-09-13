using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Animal : VisionAI, IInteractable
{
    [SerializeField] protected string creatureName = "Default Creature";
    [SerializeField] protected bool isFriendly = true;
    [SerializeField] protected float speed = 4.0f;
    [SerializeField] protected bool canIdle = true;
    [SerializeField] protected bool canWander = true;
    [SerializeField] protected bool canChase = true;
    [SerializeField] protected bool canFlee = false;
    [SerializeField] protected bool canSleep = false;
    [SerializeField] protected FacingDirection startFacingDirection = FacingDirection.Down;
    protected Animator animator;
    protected GameObject heartBubble;
    protected Rigidbody2D myRigidbody;

    private Idle idleBehavior;
    private Wander wanderBehavior;
    private Chase chaseBehavior;
    private Flee fleeBehavior;
    private Sleep sleepBehavior;
    private Interact interactBehavior;
    private readonly List<AnimalState> cyclicBehaviors = new();
    private AnimalState currentBehavior;
    private bool wantsPat;

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

        facingDirection = startFacingDirection;
        ChangeAnim(GetDirection());

        wantsPat = isFriendly;
        interactBehavior = new Interact(this, animator, heartBubble);
        if (canChase)
            chaseBehavior = new Chase(this, animator, heartBubble, transform, myRigidbody, speed);
        if (canFlee)
            fleeBehavior = new Flee(this, animator, transform, myRigidbody, speed);
        if (canWander)
        {
            wanderBehavior = new Wander(this, animator, myRigidbody, transform, speed);
            cyclicBehaviors.Add(wanderBehavior);
        }
        if (canSleep)
        {
            sleepBehavior = new Sleep(this, animator);
            cyclicBehaviors.Add(sleepBehavior);
        }
        if (canIdle)
        {
            idleBehavior = new Idle(this, animator);
            cyclicBehaviors.Add(idleBehavior);
        }
    }

    void FixedUpdate()
    {
        if (currentBehavior != null && !currentBehavior.isExitingState)
            currentBehavior.UpdateState();
        else
        {
            if (cyclicBehaviors.Count > 0)
            {
                ChangeBehavior(cyclicBehaviors[Random.Range(0, cyclicBehaviors.Count)]);
            }
            else { Debug.LogError("No behaviors available."); }
        }
    }

    public void Interact(GameObject player)
    {
        if (isFriendly)
        {
            wantsPat = false;
            ChangeBehavior(interactBehavior);
        }
    }

    protected override void DiscoverPlayer()
    {

        if (canChase && isFriendly && wantsPat)
        {
            chaseBehavior.SetTargetPlayer(targetPlayer);
            ChangeBehavior(chaseBehavior);
        }
        else if (canFlee && !isFriendly)
        {
            ChangeBehavior(fleeBehavior);
        }
    }

    protected override void LoseSight()
    {
        if (canIdle)
        {
            ChangeBehavior(idleBehavior);
        }
    }

    protected override void PlayerInVisibilityRange()
    {
        if (canChase && isFriendly && wantsPat)
        {
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
        if (currentBehavior == newBehavior) return;

        currentBehavior?.ExitState();
        newBehavior?.EnterState();
        currentBehavior = newBehavior;
    }

    public void ChangeAnim(Vector2 direction)
    {
        if (animator == null) return;

        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);
    }

    public void UpdateDirection(Vector2 direction)
    {
        facingDirection = Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
            ? (direction.x > 0 ? FacingDirection.Right : FacingDirection.Left)
            : (direction.y > 0 ? FacingDirection.Up : FacingDirection.Down);
    }

    public Vector3 GetDirection()
    {
        return facingDirection switch
        {
            FacingDirection.Up => Vector3.up,
            FacingDirection.Down => Vector3.down,
            FacingDirection.Left => Vector3.left,
            FacingDirection.Right => Vector3.right,
            _ => Vector3.zero,
        };
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Animal collided with " + collision.gameObject.name);
        currentBehavior?.OnCollisionEnter2D(collision);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        currentBehavior?.OnCollisionStay2D(collision);
    }
}
