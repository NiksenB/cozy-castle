using UnityEngine;

public class NPCIdle : NPCState
{
    private float idleDuration = 2.0f;

    public NPCIdle(NPC npc, Animator animator) : base(npc, animator)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        animator.SetBool("isMoving", false);
        idleDuration = Random.Range(5.0f, 10.0f);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (startTime < Time.time - idleDuration)
        {
            ExitState();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}