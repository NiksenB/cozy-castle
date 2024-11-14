using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Sprite princess;
    public Sprite princess_right_1;
    public Sprite princess_right_2;
    public float speed = 100;

    private SpriteRenderer mySpriteRenderer;
    private bool step;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 tempVect = new Vector3(h, v, 0);
        tempVect = tempVect.normalized * speed * Time.deltaTime;

        mySpriteRenderer.flipX = h < 0;
        UpdateSprite(h);
        rb.MovePosition(rb.transform.position + tempVect);
    }

    public void UpdateSprite(float horizontalMov)
    {
        if (horizontalMov == 0) 
        {
            mySpriteRenderer.sprite = princess;
        } 
        else 
        {
            if (step) {
                step = false;
                mySpriteRenderer.sprite = princess_right_2;
            } else 
            {
                step = true;
                mySpriteRenderer.sprite = princess_right_1;
            }
        }

        gameObject.GetComponent<SpriteRenderer>().sprite = mySpriteRenderer.sprite;
    }
}
