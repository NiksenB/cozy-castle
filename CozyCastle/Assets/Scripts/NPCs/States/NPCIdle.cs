using Unity.VisualScripting;
using UnityEngine;

public class NPCIdle : NPCState
{
    private float idleDuration = 2.0f;
    private bool isTemporary = false;

    public NPCIdle(NPC npc, Animator animator, bool isTemporary) : base(npc, animator)
    {
        this.isTemporary = isTemporary;
    }

    public override void EnterState()
    {
        base.EnterState();
        animator.SetBool("isMoving", false);
        if (isTemporary)
        {
            idleDuration = Random.Range(5.0f, 10.0f);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (isTemporary && startTime < Time.time - idleDuration)
        {
            ExitState();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}