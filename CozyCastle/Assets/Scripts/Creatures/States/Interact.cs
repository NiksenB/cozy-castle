using UnityEngine;

public class Interact : AnimalState
{
    private GameObject heartBubble;
    public Interact(Animal animal, Animator animator, GameObject heartBubble) : base(animal, animator)
    {
        this.heartBubble = heartBubble;
    }

    public override void EnterState()
    {
        base.EnterState();
        heartBubble.SetActive(true);         
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Debug.Log("Interact State Update");
        if (Time.time > startTime + 1.0f)
        {
            ExitState();
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Interact State");
        base.ExitState();
        heartBubble.SetActive(false);
    }
}
