using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class InteractableMapRegistrar : MonoBehaviour
{
    private Tilemap _tilemap;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();

        if (GameManager.gameInstance != null && GameManager.gameInstance.tileManager != null)
        {
            GameManager.gameInstance.tileManager.RegisterInteractableMap(_tilemap);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.gameInstance != null && GameManager.gameInstance.tileManager != null)
        {
            GameManager.gameInstance.tileManager.UnregisterInteractableMap(_tilemap);
        }
    }
}