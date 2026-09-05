using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameInstance;
    public static event System.Action<Player> OnPlayerRegistered;
    
    public TileManager tileManager;
    private Player _player;
    
    public void Awake()
    {
        if (gameInstance != null && gameInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        gameInstance = this;
        DontDestroyOnLoad(gameObject);

        tileManager = GetComponent<TileManager>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDestroy()
    {
        if (gameInstance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    public void RegisterPlayer(Player player)
    {
        _player = player;
        OnPlayerRegistered?.Invoke(player);
    }

    public void UnregisterPlayer(Player player)
    {
        if (_player == player)
        {
            _player = null;
        }
    }
    
    public Player Player => _player;
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded  scene: " + scene.name);
    }
}
