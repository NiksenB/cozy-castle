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

            playerPositionValue.initialValue = playerPosition;

            // Load the scene using the scene index or name
            if (sceneToLoadIndex >= 0)
            {
                SceneManager.LoadScene(sceneToLoadIndex);
            }
            else if (!string.IsNullOrEmpty(sceneToLoadName))
            {
                SceneManager.LoadScene(sceneToLoadName);
            }
        }
    }
}
