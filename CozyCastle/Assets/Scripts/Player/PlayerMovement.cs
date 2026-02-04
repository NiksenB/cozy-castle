using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Animator animator;
    public GameObject wand;
    private Vector3 direction;
    private Rigidbody2D myRigidbody;
    private Transform myTransform;
    public VectorValue startingPosition;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        if (myRigidbody == null) { Debug.LogError("Rigidbody2D component not found on PlayerMovementScript."); }
        if (myTransform == null) { Debug.LogError("Transform component not found on PlayerMovementScript."); }
        if (startingPosition.initialValue != startingPosition.defaultValue)
        {
            transform.position = startingPosition.initialValue;
        }
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

    public void FixedUpdate()
    {
        if (myRigidbody != null)
        {
            direction.Normalize(); 
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
