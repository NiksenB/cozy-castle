using UnityEngine;

public class NPCTalk : NPCState
{
    private GameObject _player;
    private Message _msg;
    private PlayerDialogueManager _dialogueManager;
    
    public NPCTalk(NPC npc, Animator animator, Message msg) : base(npc, animator)
    {
        _msg = msg;
    }

    public void SetPlayer(GameObject player)
    {
        _player = player;
        _dialogueManager  = player.gameObject.GetComponent<PlayerDialogueManager>();
    }

    public override void EnterState()
    {
        base.EnterState();
        
        animator.SetBool("isMoving", false);
        npc.ChangeAnim(Vector2.zero);
        npc.FaceTarget(_player.transform);

        if (!npc.HasEyesOnPlayer())
        {
            _dialogueManager.SetDialogue(new [] { "Oh, hello!" });
        }
        else
        {
            _dialogueManager.SetDialogue(_msg.GetLines());
        }
        _dialogueManager.AdvanceDialogue();
    }

    public void AdvanceDialogue()
    {
        Debug.Log("NPC " + npc.name + " advancing dialogue.");
        _dialogueManager.AdvanceDialogue();
    }

    public override void ExitState()
    {
        base.ExitState();
        _dialogueManager.EndDialogue();
    }
}