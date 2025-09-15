using UnityEngine;
using UnityEngine.UI;

public class MessageHandler : MonoBehaviour
{
    private GameObject dialogueBox;
    private Text dialogueText;
    [SerializeField] private string[] defaultLines;
    private string[] currentLines;
    private int currentLineIndex = 0;
    public bool isActive = false;
    public bool IsActive() => isActive;

    private void Awake()
    {
        dialogueBox = GameObject.Find("DialogueBox");
        dialogueText = dialogueBox.GetComponentInChildren<Text>();
        currentLines = defaultLines;
    }

    void Start()
    {
        dialogueBox.SetActive(false);
        if (currentLines.Length == 0)
        {
            Debug.LogWarning("Dialogue lines are not set.");
        }
    }

    public void SetDialogue(string[] newDialogue)
    {
        currentLines = newDialogue; 
        currentLineIndex = 0;
    }

    public void ResetDialogue()
    {
        currentLines = defaultLines; 
    }

    public void StartOrResumeDialogue()
    {
        isActive = true;
        if (!dialogueBox.activeSelf)
        {
            currentLineIndex = 0;
            if (currentLines.Length == 0)
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
