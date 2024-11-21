using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;

    // Called when the game starts
    private void Awake()
    {
        inventory = new Inventory(24);
    }

    public void DropItem(Collectable item)
    {
        Vector2 spawnLocation = transform.position;

        Vector2 spawnOffset = Random.insideUnitCircle * 2f ;
        
        // Spawn in world.
        Collectable droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);

        droppedItem.rigidbody2d.AddForce(spawnOffset * 3f, ForceMode2D.Impulse);
    }
}
