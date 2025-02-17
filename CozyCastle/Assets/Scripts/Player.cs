using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;

    // Called when the game starts
    private void Awake()
    {
        inventory = new Inventory(24);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3Int position = new ((int)transform.position.x, (int)transform.position.y, 0);
            
            if (GameManager.gameInstance.tileManager.IsInteractable(position))
            {
                GameManager.gameInstance.tileManager.SetInteracted(position);
            }
        }
    }

    public void DropItem(Item item)
    {
        //TODO ideally enable drag-and-drop
        float dropRadius = 1.5f;
        float minDistance = 0.5f;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minDistance, dropRadius);
        Vector2 spawnOffset = randomDirection * randomDistance;
        Vector2 spawnLocation = transform.position + (Vector3)spawnOffset;
        
        // Spawn in world.
        float randomRotation = Random.Range(0f, 360f);
        Item droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.Euler(0f, 0f, randomRotation));
        
        droppedItem.rigidbody2d.AddForce(spawnOffset.normalized * 2f, ForceMode2D.Impulse);

        droppedItem.rigidbody2d.linearDamping = 4f;         // Higher linear drag to stop movement faster
        droppedItem.rigidbody2d.angularDamping = 2f;  // Higher angular drag to stop rotation faster
        
        Debug.Log("Player removed inventory item " + item.data.itemName);
    }
}
