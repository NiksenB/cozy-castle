using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    public IInteractable CurrentInteractionTarget { get; private set; }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            CurrentInteractionTarget = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            if (interactable == CurrentInteractionTarget)
            {
                CurrentInteractionTarget = null;
            }
        }
    }
}
