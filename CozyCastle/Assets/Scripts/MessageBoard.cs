using UnityEngine;

public class MessageBoard : TextMessage, IInteractable
{
    public void Interact(GameObject player)
    {
        StartOrResumeDialogue();
    }
}