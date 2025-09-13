using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
public class NPC : VisionAI, IInteractable
{
    [SerializeField] protected string npcName = "Alice Robertson Carlisle";
    [SerializeField] protected bool followsPlayer = true;
    [SerializeField] protected bool canSleep = false;
    [SerializeField] protected bool hasMessage = false;
    [SerializeField] protected FacingDirection startFacingDirection = FacingDirection.Down;
    [SerializeField] protected float speed = 1.5f;
    protected Animator animator;
    protected GameObject heartBubble;
    protected GameObject exclamationBubble;
    protected Rigidbody2D myRigidbody;
    protected TextMessage dialogue;

    private NPCIdle idleBehavior;
    private NPCFollow followBehavior;
    private NPCTalk talkBehavior;
    private NPCPatrol patrolBehavior;
    private NPCSleep sleepBehavior;
    private NPCState currentBehavior;
    private EdgeCollider2D patrolRoute;
    private readonly List<NPCState> cyclicBehaviors = new();

    void Start()
    {
        myRigidbody = GetComponentInChildren<Rigidbody2D>();
        if (myRigidbody == null) Debug.LogError("Rigidbody2D component not found on the object.");

        animator = GetComponentInChildren<Animator>();
        if (animator == null) Debug.LogError("Animator component not found on the object.");

        if (GetComponentInParent<EdgeCollider2D>() != null)
        {
            patrolRoute = GetComponentInParent<EdgeCollider2D>();
        }
        if (patrolRoute == null && !followsPlayer)
        {
            Debug.LogWarning(npcName + " has no patrol route defined and does not follow the player. It will remain idle.");
        }

        if (transform.Find("LoveBubble") != null)
        {
            heartBubble = transform.Find("LoveBubble").gameObject;
            heartBubble.SetActive(false);
        }
        else Debug.LogError("Heart Bubble prefab is not assigned. Please assign it in the inspector.");

        if (transform.Find("ExclamationBubble") != null)
        {
            exclamationBubble = transform.Find("ExclamationBubble").gameObject;
            exclamationBubble.SetActive(false);
        }
        else Debug.LogError("Exclamation Bubble prefab is not assigned. Please assign it in the inspector.");

        if (GetComponentInChildren<TextMessage>() != null)
        {
            dialogue = GetComponentInChildren<TextMessage>();
        }
        else Debug.LogError("Dialogue component not found. Please add a TextMessage component as a child.");

        facingDirection = startFacingDirection;
        ChangeAnim(GetDirection());

        SetupBehaviors();
    }

    private void SetupBehaviors()
    {
        talkBehavior = new NPCTalk(this, animator, dialogue);

        if (canSleep)
        {
            sleepBehavior = new NPCSleep(this, animator);
            cyclicBehaviors.Add(sleepBehavior);
        }

        if (followsPlayer)
        {
            followBehavior = new NPCFollow(this, animator, heartBubble, transform, myRigidbody);
            currentBehavior = followBehavior;
        }
        else if (patrolRoute != null)
        {
            patrolBehavior = new NPCPatrol(this, animator, myRigidbody, transform, speed, patrolRoute);
            currentBehavior = patrolBehavior;
            cyclicBehaviors.Add(patrolBehavior);
        }
        else
        {
            idleBehavior = new NPCIdle(this, animator);
            currentBehavior = idleBehavior;
            cyclicBehaviors.Add(idleBehavior);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (currentBehavior != null && !currentBehavior.isExitingState)
        {
            currentBehavior.UpdateState();
        }
        else if (cyclicBehaviors.Count > 0)
        {
            int nextIndex = Random.Range(0, cyclicBehaviors.Count);
            ChangeBehavior(cyclicBehaviors[nextIndex]);
        }
        else
        {
            Debug.LogWarning(npcName + " has no behaviors to switch to.");
        }
    }

    public void Interact(GameObject player)
    {
        if (currentBehavior == talkBehavior)
        {
            talkBehavior.NextLine();
            return;
        }
        Debug.Log(player.name + " is interacting with " + npcName);
        if (exclamationBubble != null) exclamationBubble.SetActive(false);
        talkBehavior.SetPlayer(player);
        ChangeBehavior(talkBehavior);
    }

    protected override void DiscoverPlayer()
    {
        base.DiscoverPlayer();
        Debug.Log(npcName + " has discovered the player.");
        if (followsPlayer && currentBehavior != talkBehavior)
        {
            followBehavior.SetTarget(targetPlayer);
            StartCoroutine(ShowBubble(1.0f, exclamationBubble));
            ChangeBehavior(followBehavior);
        }
        else if (hasMessage && currentBehavior != talkBehavior)
        {
            Debug.Log(npcName + " has a message.");
            exclamationBubble.SetActive(true);
        }
    }

    protected override void LoseSight()
    {
        base.LoseSight();
        Debug.Log(npcName + " has lost sight of the player.");
        if (followsPlayer && currentBehavior != talkBehavior)
        {
            StartCoroutine(ShowBubble(1.0f, exclamationBubble));
            ChangeBehavior(idleBehavior);
        }
        exclamationBubble.SetActive(false);
    }

    protected override void PlayerInVisibilityRange()
    {
        if (hasMessage && currentBehavior != talkBehavior)
        {
            exclamationBubble.SetActive(true);
        }
    }

    protected override bool IsAlert()
    {
        return currentBehavior != sleepBehavior;
    }

    public void ChangeBehavior(NPCState newBehavior)
    {
        if (currentBehavior == newBehavior) return;

        currentBehavior?.ExitState();
        currentBehavior = newBehavior;
        newBehavior?.EnterState();
    }

    public void ChangeAnim(Vector2 direction)
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator is null, cannot change animation.");
            return;
        }

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

    private void UpdateFacingDirection(Vector3 sourceVector, Vector3 targetVector)
    {
        Vector3 direction = targetVector - sourceVector;
        facingDirection = Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
            ? (direction.x > 0 ? FacingDirection.Right : FacingDirection.Left)
            : (direction.y > 0 ? FacingDirection.Up : FacingDirection.Down);
    }

    public void FacePlayer(GameObject player)
    {
        UpdateFacingDirection(transform.position, player.transform.position);
        ChangeAnim(GetDirection());
    }

    public IEnumerator ShowBubble(float duration, GameObject bubble)
    {
        if (bubble == null) yield break;
        yield return StartCoroutine(ShowBubbleCoroutine(bubble, duration));
    }

    private IEnumerator ShowBubbleCoroutine(GameObject bubble, float duration)
    {
        bubble.SetActive(true);
        yield return new WaitForSeconds(duration);
        bubble.SetActive(false);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(npcName + " collided with " + collision.gameObject.name);
        currentBehavior?.OnCollisionEnter2D(collision);
    }
}
