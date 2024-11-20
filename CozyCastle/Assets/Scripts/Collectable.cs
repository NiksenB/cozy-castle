using UnityEngine;

public class Collectable : MonoBehaviour
{
    public CollectableType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player) 
        {
            player.inventory.Add(type, IsStackable(type));
            Destroy(this.gameObject);
        }
    }

    private bool IsStackable(CollectableType type)
    {
        return type switch
        {
            CollectableType.COFFEE => true,
            _ => false,
        };
    }
}

public enum CollectableType
{
    NONE, COFFEE
}