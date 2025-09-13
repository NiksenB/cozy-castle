using UnityEngine;

public class NPCSleep : NPCState
{
    public NPCSleep(NPC npc, Animator animator) : base(npc, animator)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        animator.SetBool("isMoving", false);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        return; // NPC is sleeping, do nothing
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}