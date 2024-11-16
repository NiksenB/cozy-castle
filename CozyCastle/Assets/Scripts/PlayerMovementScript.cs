using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;

    public void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h, v, 0);
        transform.position += direction * speed * Time.deltaTime;
    }
}
