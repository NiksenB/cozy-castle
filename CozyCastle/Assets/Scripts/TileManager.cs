using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile overgrownTile;


    void Start()
    {
        HideInteractableTiles();
    }

    public void HideInteractableTiles()
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

    public void RefreshTilemapReference()
    {
        GameObject tilemapObj = GameObject.Find("InteractableMap");
        if (tilemapObj != null)
        {
            interactableMap = tilemapObj.GetComponent<Tilemap>();
            if (interactableMap == null)
            {
                Debug.LogError("InteractableMap GameObject found, but no Tilemap component attached.");
            }
        }
        else
        {
            Debug.LogWarning("InteractableMap GameObject not found in the scene.");
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

