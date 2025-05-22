using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed;
    public Animator animator;
    public GameObject wand;
    private Vector3 direction;

    public void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        direction = new Vector3(h, v, 0);

        AnimateMovement(direction);

        bool wandEquipped = GameManager.gameInstance.player.inventoryManager.toolbar.selectedSlot.itemName == "Magic Wand";
        if (wand != null)
        {
            wand.SetActive(wandEquipped);
        }
    }

    // Called after collisions are calculated
    public void FixedUpdate()
    {
        // Move the player
        transform.position += direction * speed * Time.deltaTime;
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
            Animator wandAnimator = wand.GetComponent<Animator>();
            if (wandAnimator != null)
            {
                wandAnimator.SetTrigger("swingWand"); 
            }
        }
    }
}
