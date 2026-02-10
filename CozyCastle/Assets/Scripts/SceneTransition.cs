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

    // private void Awake()
    // {
    //     if (fadeInPanel != null)
    //     {
    //         GameObject panel = Instantiate(fadeInPanel);
    //         Destroy(panel, 1);
    //     }
    // }
    
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
        GameObject fade = null;
        if (fadeOutPanel != null)
        {
            fade = Instantiate(fadeOutPanel);
            // Move it into THIS scene explicitly, not Bootstrap's
            SceneManager.MoveGameObjectToScene(fade, gameObject.scene);
        }

        yield return new WaitForSeconds(0.6f);

        string currentScene = gameObject.scene.name;
        yield return Bootstrap.Instance.TransitionToScene(sceneToLoadName, sceneToLoadIndex, currentScene);

        if (fade != null)
            Destroy(fade);
    }
}
