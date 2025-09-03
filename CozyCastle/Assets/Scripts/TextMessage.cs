using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class TextMessage : MonoBehaviour, IInteractable
{
    private GameObject dialogueBox;
    private Text dialogueText;
    public string[] dialogueLines;

    private void Awake()
    {
        dialogueBox = GameObject.Find("DialogueBox");
        dialogueText = dialogueBox.GetComponentInChildren<Text>();
    }

    void Start()
    {
        dialogueBox.SetActive(false);
        if (dialogueLines.Length == 0)
        {
            Debug.LogWarning("Dialogue lines are not set.");
        }
    }

    public void Interact(GameObject player)
    {
        StartOrResumeDialogue();
    }

    public void StartOrResumeDialogue()
    {
        if (dialogueBox.activeSelf)
        {
            ShowNextLineElseClose();
        }
        else
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        if (dialogueLines.Length == 0)
        {
            Debug.LogWarning("No dialogue lines set for the sign.");
            return;
        }

        if (!dialogueBox.activeSelf)
        {
            Time.timeScale = 0;
            dialogueBox.SetActive(true);
            dialogueText.text = dialogueLines[0];
        }
    }

    private void EndDialogue()
    {
        if (dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void ShowNextLineElseClose()
    {
        if (dialogueLines.Length == 0)
        {
            EndDialogue();
        }

        int currentLineIndex = dialogueText.text == "" ? 0 : System.Array.IndexOf(dialogueLines, dialogueText.text);

        if (currentLineIndex < 0 || currentLineIndex >= dialogueLines.Length - 1)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = dialogueLines[currentLineIndex + 1];
    }
}
