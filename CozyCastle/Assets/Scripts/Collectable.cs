using UnityEngine;

public class Collectable : MonoBehaviour
{
    public CollectableType type;
    public Sprite icon;
    public Rigidbody2D rigidbody2d;
    public bool isStackable;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        
        if (player) 
        {
            player.inventory.AddToInventory(this);
            Destroy(this.gameObject);
        }
    }
}

public enum CollectableType
{
    NONE, 
    COFFEE, 
    TOMATO,
}