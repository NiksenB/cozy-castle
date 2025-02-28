using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameInstance;
    public ItemManager itemManager;
    public TileManager tileManager;

    public void Awake()
    {
        if (gameInstance != null && gameInstance != this)
        {
            Destroy(gameObject);
        }
        else 
        {
            gameInstance = this;
        }

        // Is preserved between scenes.
        DontDestroyOnLoad(gameObject);
        
        itemManager = GetComponent<ItemManager>();
        tileManager = GetComponent<TileManager>();
    }
}
