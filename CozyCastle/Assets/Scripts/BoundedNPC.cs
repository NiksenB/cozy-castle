using UnityEngine;

public class BoundedNPC : Interactable
{

    public Collider2D boundsCollider;
    public float speed = 1.5f;
    private Vector3 directionVector;
    private Transform myTransform;
    private Rigidbody2D myRigidbody;
    private Animator anim;

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

    void FixedUpdate()
    {
        if (!isPlayerInRange) Move();
    }

    private void Move()
    {
        Vector3 targetPosition = myTransform.position + speed * Time.deltaTime * directionVector;
        if (boundsCollider.bounds.Contains(targetPosition))
        {
            myRigidbody.MovePosition(targetPosition);
        }
        else
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0: // Up
                directionVector = Vector3.up;
                break;
            case 1: // Down
                directionVector = Vector3.down;
                break;
            case 2: // Left
                directionVector = Vector3.left;
                break;
            case 3: // Right
                directionVector = Vector3.right;
                break;
        }
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        anim.SetFloat("horizontal", directionVector.x);
        anim.SetFloat("vertical", directionVector.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 temp = directionVector;
        ChangeDirection();
        int attempts = 0;
        while (directionVector == temp && attempts < 20)
        {
            attempts++;
            ChangeDirection();
        }
    }
}
