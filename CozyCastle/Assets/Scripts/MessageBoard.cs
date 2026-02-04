using UnityEngine;

public class MessageBoard : Message, IInteractable
{
    public void Interact(GameObject player)
    {
        PlayerDialogueManager _dialogueManager  = player.GetComponent<PlayerDialogueManager>();
        if (!_dialogueManager.IsActive())
        {
            _dialogueManager.SetDialogue(GetLines());
        }
        _dialogueManager.AdvanceDialogue();
    }
}