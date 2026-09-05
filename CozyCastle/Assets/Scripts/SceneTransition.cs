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

            string currentScene = gameObject.scene.name;

            // Run the whole sequence on Bootstrap (persistent, DontDestroyOnLoad).
            Bootstrap.Instance.StartCoroutine(
                DoTransition(sceneToLoadName, sceneToLoadIndex, currentScene));
        }
    }

    private IEnumerator DoTransition(string sceneName, int sceneIndex, string sceneToUnload)
    {
        GameObject fadeIn = null;
        if (fadeInPanel != null)
        {
            fadeIn = Instantiate(fadeInPanel);
            // Must survive the coming scene unload.
            DontDestroyOnLoad(fadeIn);
        }

        yield return new WaitForSeconds(0.6f);

        if (fadeIn != null)
            Destroy(fadeIn);

        GameObject fadeOut = null;
        if (fadeOutPanel != null)
        {
            fadeOut = Instantiate(fadeOutPanel);
            DontDestroyOnLoad(fadeOut); 
        }

        yield return Bootstrap.Instance.TransitionToScene(sceneName, sceneIndex, sceneToUnload);

        yield return new WaitForSeconds(0.6f);

        if (fadeOut != null)
            Destroy(fadeOut);
    }
}