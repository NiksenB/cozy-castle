using UnityEngine;

public class BoundedNPC : VisionAI, IInteractable
{

    [SerializeField] TextMessage dialogue;
    public Collider2D boundsCollider;
    public float speed = 1.5f;
    private Vector3 directionVector;

    private Transform myTransform;
    private Rigidbody2D myRigidbody;
    private Animator anim;
    private bool awaitInteract;

    void Awake()
    {
        myTransform = GetComponent<Transform>();
        Debug.Assert(myTransform != null, "Transform component not found on BoundedNPC.");
        myRigidbody = GetComponent<Rigidbody2D>();
        Debug.Assert(myRigidbody != null, "Rigidbody2D component not found on BoundedNPC.");
        anim = GetComponent<Animator>();
        Debug.Assert(anim != null, "Animator component not found on BoundedNPC.");
    }

    void Start()
    {
        ChangeDirection();
    }

    public void Interact(GameObject player)
    {
        FacePlayer(player);
        anim.SetBool("isMoving", false);
        dialogue.Interact(player);
    }

    void FixedUpdate()
    {
        if (awaitInteract) return;

        anim.SetBool("isMoving", true);
        Move();
    }

    private void Move()
    {
        Vector3 targetPosition = myTransform.position + speed * Time.deltaTime * GetDirectionVector();
        if (boundsCollider.bounds.Contains(targetPosition))
        {
            myRigidbody.MovePosition(targetPosition);
        }
        else
        {
            ChangeDirection();
        }
    }

    private Vector3 GetDirectionVector()
    {
        switch (facingDirection)
        {
            case FacingDirection.Up:
                return Vector3.up;
            case FacingDirection.Down:
                return Vector3.down;
            case FacingDirection.Left:
                return Vector3.left;
            case FacingDirection.Right:
                return Vector3.right;
            default:
                return Vector3.zero;
        }
    }

    void FacePlayer(GameObject player)
    {
        Vector3 playerDirection = player.transform.position - new Vector3(myTransform.position.x, myTransform.position.y);

        if (Mathf.Abs(playerDirection.x) > Mathf.Abs(playerDirection.y))
        {
            facingDirection = playerDirection.x > 0 ? FacingDirection.Right : FacingDirection.Left;
        }
        else
        {
            facingDirection = playerDirection.y > 0 ? FacingDirection.Up : FacingDirection.Down;
        }
        UpdateAnimation();
    }

    void ChangeDirection()
    {
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0:
                facingDirection = FacingDirection.Up;
                break;
            case 1:
                facingDirection = FacingDirection.Down;
                break;
            case 2:
                facingDirection = FacingDirection.Left;
                break;
            case 3:
                facingDirection = FacingDirection.Right;
                break;
        }
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        anim.SetFloat("horizontal", facingDirection == FacingDirection.Left ? -1 : facingDirection == FacingDirection.Right ? 1 : 0);
        anim.SetFloat("vertical", facingDirection == FacingDirection.Down ? -1 : facingDirection == FacingDirection.Up ? 1 : 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            awaitInteract = true;
            anim.SetBool("isMoving", false);
            FacePlayer(collision.gameObject);
            return;
        }
        Vector3 temp = directionVector;
        ChangeDirection();
        int attempts = 0;
        while (directionVector == temp && attempts < 20)
        {
            attempts++;
            ChangeDirection();
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) awaitInteract = false;
    }
}
