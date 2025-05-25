using UnityEngine;
public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        // Ensure this GameObject is not destroyed when loading a new scene
        DontDestroyOnLoad(gameObject);
    }
}