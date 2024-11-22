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

    public void DropItem(Collectable item)
    {
        //TODO fix drop location
        //TODO ideally enable drag-and-drop
        Vector2 spawnLocation = transform.position;

        Vector2 spawnOffset = Random.insideUnitCircle * 2f ;
        
        // Spawn in world.
        Collectable droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);

        droppedItem.rigidbody2d.AddForce(spawnOffset * 3f, ForceMode2D.Impulse);
    }
}
