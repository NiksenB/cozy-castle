using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject dialogueBox;
    public Text dialogueText;
    public string[] dialogueLines;
    private bool isPlayerInRange = false;


    void Start()
    {
        dialogueBox.SetActive(false);
        if (dialogueLines.Length == 0)
        {
            Debug.LogWarning("Dialogue lines are not set for the Sign script.");
        }
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            if (dialogueBox.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
                {
                    ShowNextLineElseClose();
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartDialogue();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Escape key pressed, closing dialogue.");
                EndDialogue();
            }
        }
        else
        {
            EndDialogue();
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

        Debug.Log($"Current line index: {currentLineIndex}, Total lines: {dialogueLines.Length}");

        dialogueText.text = dialogueLines[currentLineIndex + 1];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueBox.SetActive(false);
        }
    }
}
