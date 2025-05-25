using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameInstance;
    public TileManager tileManager;
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
            DontDestroyOnLoad(gameObject);
        }
        
        tileManager = GetComponent<TileManager>();
        player = FindFirstObjectByType<Player>();
    }
}
