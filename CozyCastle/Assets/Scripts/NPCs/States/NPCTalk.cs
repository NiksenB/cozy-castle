using UnityEngine;

public class NPCTalk : NPCState
{
    private GameObject player;
    private MessageHandler dialogue;
    private bool dialogueNeedsReset = false;
    public NPCTalk(NPC npc, Animator animator, MessageHandler dialogue) : base(npc, animator)
    {
        this.dialogue = dialogue;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public override void EnterState()
    {
        base.EnterState();
        animator.SetBool("isMoving", false);
        npc.ChangeAnim(Vector2.zero);
        npc.FaceTarget(player.transform);

        if (!npc.HasEyesOnPlayer())
        {
            SetTempDialogue(new string[] { "Oh, hello!" });
        }
        dialogue.StartOrResumeDialogue();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (!dialogue.IsActive())
        {
            ExitState();
        }
    }

    public void SetTempDialogue(string[] lines)
    {
        dialogue.SetDialogue(lines);
        dialogueNeedsReset = true;
    }

    public void ResetDialogue()
    {
        dialogue.ResetDialogue();
    }


    public void NextLine()
    {
        Debug.Log("NPC " + npc.name + " advancing dialogue.");
        dialogue.StartOrResumeDialogue();
    }

    public override void ExitState()
    {
        base.ExitState();
        dialogue.EndDialogue();
        if (dialogueNeedsReset)
        {
            dialogue.ResetDialogue();
            dialogueNeedsReset = false;
        }
    }
}