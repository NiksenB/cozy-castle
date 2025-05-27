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
                player.playerInventoryManager.AddItem("toolbar", item.data);
                Destroy(gameObject);
            } else
            {
                Debug.LogWarning("Collectable does not have an Item component attached.");
            }
        }
    }
}