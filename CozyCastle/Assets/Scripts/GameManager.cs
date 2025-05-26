using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
        player = FindFirstObjectByType<Player>();
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (tileManager != null)
        {
            tileManager.RefreshTilemapReference();
            tileManager.HideInteractableTiles();
        }
    }

}
