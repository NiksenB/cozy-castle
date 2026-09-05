using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile overgrownTile;

    public void RegisterInteractableMap(Tilemap map)
    {
        interactableMap = map;
        Debug.Log($"[TileManager] Registered InteractableMap: {map.name}");
        HideInteractableTiles();
    }

    public void UnregisterInteractableMap(Tilemap map)
    {
        // Only clear if this is the map we're currently tracking — during
        // the load-before-unload overlap, the new scene's map may already
        // have registered and overwritten this by the time the old scene's
        // map is destroyed. Guard against clobbering the live reference.
        if (interactableMap == map)
        {
            interactableMap = null;
        }
    }

    public void HideInteractableTiles()
    {
        if (interactableMap == null)
        {
            Debug.LogWarning("[TileManager] HideInteractableTiles: no InteractableMap registered.");
            return;
        }

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
        if (interactableMap == null)
        {
            Debug.LogWarning("[TileManager] SetInteracted: no InteractableMap registered.");
            return;
        }

        Debug.Log($"Setting interacted tile at position: {position}");
        interactableMap.SetTile(position, overgrownTile);
    }

    public string GetTileName(Vector3Int position)
    {
        if (interactableMap != null)
        {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null)
                return tile.name;
        }
        return "";
    }
}