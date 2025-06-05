using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed;
    public Animator animator;
    public GameObject wand;
    private Vector3 direction;
    private Rigidbody2D myRigidbody;
    public VectorValue startingPosition;

    private void Start()
    {
        // TODO remove this, it is only for testing purposes
        VectorValue pos = Resources.Load<VectorValue>("PlayerPosition"); 
        if (pos != null) pos.value = Vector3.zero;

        myRigidbody = GetComponent<Rigidbody2D>();
        if (myRigidbody == null)
        {
            Debug.LogError("Rigidbody2D component not found on PlayerMovementScript.");
        }
        myRigidbody.MovePosition(startingPosition.value);
    }

    public void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        direction = new Vector3(h, v, 0);

        AnimateMovement(direction);

        InventoryData toolbarData = GameManager.gameInstance.player.playerInventoryManager.toolbar;
        bool wandEquipped = false;
        if (toolbarData != null && toolbarData.selectedSlot != null)
        {
            if (toolbarData.selectedSlot.itemName == "Magic Wand")
            {
                wandEquipped = true;
            }
        }
        if (wand != null)
        {
            wand.SetActive(wandEquipped);
        }
    }

    // Called after collisions are calculated
    public void FixedUpdate()
    {
        // Move the player
        //transform.position += speed * Time.deltaTime * direction;
        if (myRigidbody != null)
        {
            direction.Normalize(); // Ensure direction is normalized before multiplying by speed
            Vector3 movement = speed * Time.deltaTime * direction;
            myRigidbody.MovePosition(transform.position + movement);
        }
        else
        {
            Debug.LogError("Rigidbody2D is not assigned in PlayerMovementScript.");
        }
    }

    public void AnimateMovement(Vector3 direction)
    {
        if (animator != null)
        {
            if (direction.magnitude > 0)
            {
                animator.SetBool("isMoving", true);
                animator.SetFloat("horizontal", direction.x);
                animator.SetFloat("vertical", direction.y);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

    public void PlayWandSwingAnimation()
    {
        if (wand != null)
        {
            if (wand.TryGetComponent<Animator>(out var wandAnimator))
            {
                wandAnimator.SetTrigger("swingWand"); 
            }
        }
    }
}
