using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected bool isPlayerInRange = false;
    protected Vector3 collidingPlayerPosition = Vector3.down;

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            collidingPlayerPosition = other.transform.position;
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
