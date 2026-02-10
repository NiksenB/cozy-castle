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
        Debug.Log($"[GameManager] Player registered: id={player.GetInstanceID()}");
        OnPlayerRegistered?.Invoke(player);
    }

    public void UnregisterPlayer(Player player)
    {
        if (_player == player)
        {
            _player = null;
            Debug.Log($"[GameManager] Player unregistered: id={player.GetInstanceID()}");
        }
    }
    
    public Player Player => _player;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Canvas" || scene.name == "Bootstrap")
            return;

        if (tileManager != null)
        {
            tileManager.RefreshTilemapReference();
            tileManager.HideInteractableTiles();
        }
    }
    
    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     if (scene.name == "Canvas" || scene.name == "Bootstrap")
    //         return;
    //
    //     var allPlayers = FindObjectsByType<Player>(FindObjectsSortMode.None);
    //     if (allPlayers.Length > 1)
    //     {
    //         Debug.LogWarning($"[RACE] {allPlayers.Length} Player instances alive when '{scene.name}' loaded! " +
    //                          string.Join(", ", System.Array.ConvertAll(allPlayers, p => p.GetInstanceID().ToString())));
    //     }
    //
    //     var previousPlayerId = _player != null ? _player.GetInstanceID() : -1;
    //     _player = FindFirstObjectByType<Player>();
    //     Debug.LogWarning($"[GM] OnSceneLoaded('{scene.name}') -> resolved Player id={_player?.GetInstanceID()} (was {previousPlayerId})");
    //
    //     if (_player == null)
    //     {
    //         Debug.LogError("Player not found in the scene after loading.");
    //         return;
    //     }
    //
    //     if (tileManager != null)
    //     {
    //         tileManager.RefreshTilemapReference();
    //         tileManager.HideInteractableTiles();
    //     }
    //
    //     // // Re-fire so UI rebinds to the new scene's player
    //     // var inv = _player.GetComponent<PlayerInventoryManager>();
    //     // if (inv != null)
    //     //     inv.RaiseInventoriesReady();
    // }
}
