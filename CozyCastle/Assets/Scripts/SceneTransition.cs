using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int sceneToLoadIndex;
    public string sceneToLoadName;
    public Vector3 playerPosition;
    public VectorValue playerPositionValue;
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;

    private void Awake()
    {
        if (fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel);
            Destroy(panel, 1);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            if (playerPositionValue == null)
            {
                Debug.LogError("PlayerPositionValue is not assigned in SceneTransition.");
                return;
            }

            playerPositionValue.initialValue = playerPosition;

            StartCoroutine(FadeOut());

            
        }
    }

    public IEnumerator FadeOut()
    {
        if (fadeOutPanel != null)
        {
            Instantiate(fadeOutPanel);
        }
        yield return new WaitForSeconds(0.6f);

        AsyncOperation async = LoadSceneAsync();
        
        while (!async.isDone)
        {
            yield return null;
        }
    }

    private AsyncOperation LoadSceneAsync()
    {
        // Load the scene using the scene index or name
        if (sceneToLoadIndex >= 0)
        {
            return SceneManager.LoadSceneAsync(sceneToLoadIndex);
        }
        
        if (!string.IsNullOrEmpty(sceneToLoadName))
        {
            return SceneManager.LoadSceneAsync(sceneToLoadName);
        }

        return null;
    }

}
