using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    public List<IInteractable> CurrentInteractionTargets { get; private set; } = new List<IInteractable>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            if (!CurrentInteractionTargets.Contains(interactable))
            {
                CurrentInteractionTargets.Add(interactable);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            CurrentInteractionTargets.Remove(interactable);
        }
    }
}
