using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed;
    public Animator animator;
    private Vector3 direction;

    public void Update()
    {
        // Get player input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        direction = new Vector3(h, v, 0);

        AnimateMovement(direction);
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
}
