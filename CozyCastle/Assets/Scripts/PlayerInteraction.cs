using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private InteractionZone interactionZone;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            var target = interactionZone.CurrentInteractionTarget;
            Debug.Log("Interacting with " + (target != null ? target.ToString() : "nothing"));
            target?.Interact(gameObject); // gameObject = player
        }
    }
}
