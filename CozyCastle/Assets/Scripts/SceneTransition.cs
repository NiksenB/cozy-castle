using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int sceneIndex;
    public string sceneName;
    public Vector3 playerPosition;
    public VectorValue playerPositionValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPositionValue.value = playerPosition;
            Debug.Log("Transitioning to scene: " + sceneName + ", scene index: " + sceneIndex);
            Debug.Log("Player position set to: " + playerPositionValue.value);

            // Load the scene using the scene index or name
            if (sceneIndex >= 0)
            {
                SceneManager.LoadScene(sceneIndex);
            }
            else if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
