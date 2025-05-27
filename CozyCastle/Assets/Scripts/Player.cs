using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInventoryManager playerInventoryManager;
    private TileManager tileManager;
    private PlayerStats playerStats;
    private PlayerMovementScript movementScript;

    private void Awake()
    {
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        Debug.Assert(playerInventoryManager != null, "PlayerInventoryManager component is missing on Player GameObject.");
        Debug.Log("PlayerInventoryManager found: " + playerInventoryManager.name);

        movementScript = GetComponent<PlayerMovementScript>();
        Debug.Assert(movementScript != null, "PlayerMovementScript component is missing on Player GameObject.");

        playerStats = GetComponent<PlayerStats>(); 
        Debug.Assert(playerStats != null, "PlayerStats component is missing on Player GameObject.");
    }

    private void Start()
    {
        tileManager = GameManager.gameInstance.tileManager;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (tileManager != null)
            {
                Vector3Int position = new((int)transform.position.x, (int)transform.position.y, 0);

                string tileName = tileManager.GetTileName(position);

                if (!string.IsNullOrEmpty(tileName))
                {
                    if (tileName == "Interactable" && playerInventoryManager.toolbar.selectedSlot.itemName == "Magic Wand")
                    {
                        TryInteract(position);
                    }
                }
            }
        }
    }

    public void DropItem(Item item)
    {
        float dropRadius = 1.0f;
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
        for (int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }

    private void TryInteract(Vector3Int position)
    {
        Debug.Log($"Player trying to interact with tile at position: {position}");
        if (playerStats.TryUseMana(25))
        {
            movementScript.PlayWandSwingAnimation();
            StartCoroutine(DelayedInteract(position, 0.5f));
        }
    }
    
    private System.Collections.IEnumerator DelayedInteract(Vector3Int position, float delay)
    {
        yield return new WaitForSeconds(delay);
        tileManager.SetInteracted(position);
    }
}
