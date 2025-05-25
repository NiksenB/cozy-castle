using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int sceneToLoadIndex;
    public string sceneToLoadName;
    public Vector3 playerPosition;
    public VectorValue playerPositionValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            if (playerPositionValue == null)
            {
                Debug.LogError("PlayerPositionValue is not assigned in SceneTransition.");
                return;
            }

            playerPositionValue.value = playerPosition;

            // Load the scene using the scene index or name
            if (sceneToLoadIndex >= 0)
            {
                Debug.Log("Transitioning to scene: " + sceneToLoadName + ", scene index: " + sceneToLoadIndex);
                Debug.Log("Player position set to: " + playerPositionValue.value);
                SceneManager.LoadScene(sceneToLoadIndex);
            }
            else if (!string.IsNullOrEmpty(sceneToLoadName))
            {
                SceneManager.LoadScene(sceneToLoadName);
            }
        }
    }
}
