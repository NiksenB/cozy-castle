using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    // Serialize is used so we can drag the Tilemap into the Inspector.
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile overgrownTile;

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

    // public bool IsInteractable(Vector3Int position)
    // {
    //     TileBase tile = interactableMap.GetTile(position);

    //     if (tile != null)
    //     {
    //         return tile.name == "Interactable";
    //     }
    //     return false;
    // }

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

