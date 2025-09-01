using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class TextMessage : Interactable
{
    public GameObject dialogueBox;
    public Text dialogueText;
    public string[] dialogueLines;

    void Start()
    {
        dialogueBox.SetActive(false);
        if (dialogueLines.Length == 0)
        {
            Debug.LogWarning("Dialogue lines are not set.");
        }
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            if (dialogueBox.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ShowNextLineElseClose();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                StartDialogue();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Escape key pressed, closing dialogue.");
                EndDialogue();
            }
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

    private new void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.CompareTag("Player"))
        {
            dialogueBox.SetActive(false);
        }
    }
}
