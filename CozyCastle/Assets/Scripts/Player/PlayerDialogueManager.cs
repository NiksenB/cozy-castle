using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDialogueManager : MonoBehaviour
{
    
    private GameObject dialogueBox;
    private Text dialogueText;
    private string[] currentLines;
    private int currentLineIndex;
    private bool isActive;
    public bool IsActive() => isActive;
    
    private void Start()
    {
        GameObject[] objs  = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        dialogueBox = objs.FirstOrDefault(item => item.name == "DialogueBox");

        if (dialogueBox == null)
        {
            Debug.LogWarning("DialogueBox not found");
        }

        dialogueText = dialogueBox?.GetComponentInChildren<Text>();
        dialogueBox?.SetActive(false);
    }
    
    public void SetDialogue(string[] newDialogue)
    {
        currentLines = newDialogue; 
        currentLineIndex = 0;
    }

    public void AdvanceDialogue()
    {
        isActive = true;

        if (dialogueBox == null)
        {
            Debug.LogWarning("PlayerDialogueManager missing dialogueBox.");
            return;
        }

        if (!dialogueBox.activeSelf)
        {
            currentLineIndex = 0;
            if (currentLines == null || currentLines.Length == 0)
            {
                Debug.LogWarning("No dialogue lines set for this dialogue.");
                return;
            }

            Time.timeScale = 0;
            dialogueBox.SetActive(true);
            dialogueText.text = currentLines[currentLineIndex];
        }
        else
        {
            currentLineIndex++;
            ShowNextLineElseClose(currentLineIndex);
        }
    }

    public void EndDialogue()
    {
        isActive = false;
        if (dialogueBox == null)
        {
            Debug.LogWarning("PlayerDialogueManager missing dialogueBox.");
            return;
        }
        if (dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void ShowNextLineElseClose(int index)
    {
        if (currentLines.Length == 0)
        {
            EndDialogue();
        }

        if (index < 0 || index >= currentLines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = currentLines[index];
    }
}
