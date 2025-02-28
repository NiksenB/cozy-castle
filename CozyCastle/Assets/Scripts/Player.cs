using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;
    private PlayerStats playerStats;

    // Called when the game starts
    private void Awake()
    {
        inventory = new Inventory(27);
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

        droppedItem.rigidbody2d.linearDamping = 4f;     // Linear drag slows down movement
        droppedItem.rigidbody2d.angularDamping = 2f;    // Angular drag slows down rotation
        
        Debug.Log("Player removed inventory item " + item.data.itemName);
    }

    private void TryInteract(Vector3Int position)
    {
        // TODO there should be some check here to see what magic is equipped,
        // then an if-statemet that confirms we are allowed to spend that mana 
        // before tile is set to interacted. 
        playerStats.UseMana(25);
        GameManager.gameInstance.tileManager.SetInteracted(position);
    }
}
