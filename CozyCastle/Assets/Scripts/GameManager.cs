using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameInstance;
    public ItemManager itemManager;
    public TileManager tileManager;
    public UI_Manager uiManager;
    public Player player;

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
        uiManager = GetComponent<UI_Manager>();

        player = FindFirstObjectByType<Player>();
    }
}
