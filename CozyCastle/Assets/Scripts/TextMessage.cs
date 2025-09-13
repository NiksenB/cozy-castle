using UnityEngine;
using UnityEngine.UI;

public class TextMessage : MonoBehaviour
{
    private GameObject dialogueBox;
    private Text dialogueText;
    public string[] dialogueLines;
    public bool isActive = false;
    public bool IsActive() => isActive;

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

    public void StartOrResumeDialogue()
    {
        isActive = true;
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

    public void EndDialogue()
    {
        isActive = false;
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
