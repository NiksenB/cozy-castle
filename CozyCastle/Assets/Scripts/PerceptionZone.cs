using UnityEngine;

public class PerceptionZone : MonoBehaviour
{
    [SerializeField] private VisionAI visionAI;

    private void Awake()
    {
        visionAI = GetComponentInParent<VisionAI>();
        if (visionAI == null)
        {
            Debug.LogError("VisionAI component not found in parent hierarchy.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{visionAI.name} detected the player entering its perception zone.");
            visionAI.SetTarget(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{visionAI.name} detected the player exiting its perception zone.");
            visionAI.ClearTarget();
        }
    }
}
