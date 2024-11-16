using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed;
    public Animator animator;

    public void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h, v, 0);

        AnimateMovement(direction);

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
