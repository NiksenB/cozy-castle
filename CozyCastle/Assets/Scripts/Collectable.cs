using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        
        if (player) 
        {
            if (TryGetComponent(out Item item))
            {
                player.inventory.Add("backpack", item);
                Destroy(gameObject);
            }
        }
    }
}