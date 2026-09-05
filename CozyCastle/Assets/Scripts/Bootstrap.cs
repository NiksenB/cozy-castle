using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    public static Bootstrap Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator Start()
    {
        yield return SceneManager.LoadSceneAsync("Canvas", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
    }
    
    public IEnumerator TransitionToScene(string sceneName, int sceneIndex, string sceneToUnload)
    {
        // Explicitly flush the current player's inventory before transitioning.
        // This makes persistence correct regardless of Awake()/OnDestroy() ordering
        // between the outgoing and incoming Player/PlayerInventoryManager.
        var currentPlayer = GameManager.gameInstance != null ? GameManager.gameInstance.Player : null;
        if (currentPlayer != null && currentPlayer.playerInventoryManager != null)
        {
            currentPlayer.playerInventoryManager.SaveAll();
        }
        
        if (UI_Manager.instance != null)
        {
            UI_Manager.instance.UnbindChestIfBound();
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log($"Loading '{sceneName}'...");
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            Debug.Log($"Loaded '{sceneName}'");
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
        else if (sceneIndex > 0)
        {
            Debug.Log($"Loading index {sceneIndex}...");
            yield return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            Debug.Log($"Loaded index {sceneIndex}");
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
        }
        else
        {
            Debug.LogError("TransitionToScene: no valid scene specified.");
            yield break;
        }

        Debug.Log($"Unloading '{sceneToUnload}'...");
        yield return SceneManager.UnloadSceneAsync(sceneToUnload);
        Debug.Log($"Unloaded '{sceneToUnload}'");
    }
}