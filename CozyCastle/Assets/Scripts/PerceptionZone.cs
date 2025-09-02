using UnityEngine;

public class PerceptionZone : MonoBehaviour
{
    [SerializeField] private VisionAI visionAI;

    private void Awake()
    {
        visionAI = GetComponentInParent<VisionAI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            visionAI.SetTarget(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            visionAI.ClearTarget();
        }
    }
}
