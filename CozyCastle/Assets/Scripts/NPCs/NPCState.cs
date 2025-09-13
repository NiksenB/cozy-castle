using NUnit.Framework;
using UnityEngine;

public class NPCState
{
    protected NPC npc;
    protected Animator animator;
    protected float startTime = 0f;
    public bool isExitingState = false;

    public NPCState(NPC npc, Animator animator)
    {
        this.npc = npc;
        this.animator = animator;
    }

    public virtual void EnterState()
    {
        isExitingState = false;
        startTime = Time.time;
    }

    public virtual void UpdateState() { if (isExitingState) return; }

    public virtual void ExitState()
    {
        isExitingState = true;
        startTime = 0f;
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("NPC " + npc.name + " collided with " + collision.gameObject.name);
    }

    public virtual void OnTriggerEnter2D(GameObject other)
    {
        Debug.Log("NPC " + npc.name + " triggered by " + other.name);
    }
}
