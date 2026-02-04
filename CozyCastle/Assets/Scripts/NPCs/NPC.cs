using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
public class NPC : VisionAI, IInteractable
{
    [SerializeField] protected string npcName = "Alice Robertson Carlisle";
    [SerializeField] protected bool hasMessage = false;
    [SerializeField] protected bool isRandomlyIdle = true;
    [SerializeField] protected FacingDirection startFacingDirection = FacingDirection.Down;
    [SerializeField] protected float speed = 1.5f;
    protected Animator animator;
    protected GameObject heartBubble;
    protected GameObject exclamationBubble;
    protected Rigidbody2D myRigidbody;
    protected Message dialogue;
    protected NPCState currentBehavior;

    protected NPCIdle idleBehavior;
    protected NPCTalk talkBehavior;
    protected NPCPatrol patrolBehavior;
    protected EdgeCollider2D patrolRoute;
    protected readonly List<NPCState> cyclicBehaviors = new();

    protected virtual void Start()
    {
        myRigidbody = GetComponentInChildren<Rigidbody2D>();
        if (myRigidbody == null) Debug.LogError("Rigidbody2D component not found on the object.");

        animator = GetComponentInChildren<Animator>();
        if (animator == null) Debug.LogError("Animator component not found on the object.");

        if (GetComponentInParent<EdgeCollider2D>() != null)
        {
            patrolRoute = GetComponentInParent<EdgeCollider2D>();
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

        if (GetComponentInChildren<Message>() != null)
        {
            dialogue = GetComponentInChildren<Message>();
        }
        else Debug.LogError("Dialogue component not found. Please add a TextMessage component as a child.");

        facingDirection = startFacingDirection;
        ChangeAnim(GetDirection());

        SetupBehaviors();
    }

    private void SetupBehaviors()
    {
        talkBehavior = new NPCTalk(this, animator, dialogue);
        
        if (isRandomlyIdle)
        {
            idleBehavior = new NPCIdle(this, animator, isTemporary: true);
            cyclicBehaviors.Add(idleBehavior);
        } else
        {
            idleBehavior = new NPCIdle(this, animator, isTemporary: false);
        }

        if (patrolRoute != null)
        {
            patrolBehavior = new NPCPatrol(this, animator, myRigidbody, transform, speed, patrolRoute);
            currentBehavior = patrolBehavior;
            cyclicBehaviors.Add(patrolBehavior);
        }
        else
        {
            currentBehavior = idleBehavior;
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
            talkBehavior.AdvanceDialogue();
            return;
        }
        Debug.Log(player.name + " is interacting with " + npcName);
        if (exclamationBubble != null) exclamationBubble.SetActive(false);
        talkBehavior.SetPlayer(player);
        ChangeBehavior(talkBehavior);
        hasMessage = false;
    }

    protected override void DiscoverPlayer()
    {
        base.DiscoverPlayer();
        Debug.Log(npcName + " has discovered the player.");
        
        if (hasMessage && currentBehavior != talkBehavior)
        {
            Debug.Log(npcName + " has a message.");
            exclamationBubble.SetActive(true);
        }
    }

    protected override void LoseSight()
    {
        base.LoseSight();
        Debug.Log(npcName + " has lost sight of the player.");
        
        exclamationBubble.SetActive(false);
    }

    protected override void PlayerInVisibilityRange()
    {
        if (hasMessage && currentBehavior != talkBehavior)
        {
            exclamationBubble.SetActive(true);
        }
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

    public void FaceTarget(Transform target)
    {
        UpdateFacingDirection(transform.position, target.position);
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
