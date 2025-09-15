using UnityEngine;

public class MessageBoard : MessageHandler, IInteractable
{
    public void Interact(GameObject player)
    {
        StartOrResumeDialogue();
    }
}