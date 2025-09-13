using UnityEngine;

public class NPCTalk : NPCState
{
    private GameObject player;
    private TextMessage dialogue;
    public NPCTalk(NPC npc, Animator animator, TextMessage dialogue) : base(npc, animator)
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
        npc.FacePlayer(player);
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

    public void NextLine()
    {
        Debug.Log("NPC " + npc.name + " advancing dialogue.");
        dialogue.StartOrResumeDialogue();
    }

    public override void ExitState()
    {
        base.ExitState();
        dialogue.EndDialogue();
    }
}