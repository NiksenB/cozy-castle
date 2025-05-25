using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile overgrownTile;

    void Awake()
    {
        if (interactableMap == null)
        {
            Debug.LogError("Interactable Tilemap is not assigned in TileManager.");
        }
        if (hiddenInteractableTile == null)
        {
            Debug.LogError("Hidden Interactable Tile is not assigned in TileManager.");
        }
        if (overgrownTile == null)
        {
            Debug.LogError("Overgrown Tile is not assigned in TileManager.");
        }
    }

    void Start()
    {
        foreach (Vector3Int position in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null && tile.name == "Interactable_Visible")
            {
                interactableMap.SetTile(position, hiddenInteractableTile);
            }
        }
    }

    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, overgrownTile);
    }

    public string GetTileName(Vector3Int position)
    {
        if (interactableMap != null)
        {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null)
            {
                return tile.name;
            }
        }
        return "";
    }

}

