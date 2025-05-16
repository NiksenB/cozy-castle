using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryManager inventory;
    private PlayerStats playerStats;

    // Called when the game starts
    private void Awake()
    {
        inventory = GetComponent<InventoryManager>();
    }

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>(); 
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats component is missing from Player!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3Int position = new ((int)transform.position.x, (int)transform.position.y, 0);
            
            if (GameManager.gameInstance.tileManager.IsInteractable(position))
            {
                TryInteract(position);
            }
        }
    }

    public void DropItem(Item item)
    {
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

        droppedItem.rigidbody2d.linearDamping = 4f;     // Linear drag slows down movement
        droppedItem.rigidbody2d.angularDamping = 2f;    // Angular drag slows down rotation
        
        Debug.Log("Player removed inventory item " + item.data.itemName);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for(int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        } 
    }


    private void TryInteract(Vector3Int position)
    {
        if (playerStats.TryUseMana(25)) 
        {
            GameManager.gameInstance.tileManager.SetInteracted(position);
        }
    }
}
