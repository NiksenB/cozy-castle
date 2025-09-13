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
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        // Wait until animation is finished
        if (animal.IsNonStandardAnimationPlaying()) { return; }
        if (heartBubble.activeSelf == false)
        {
            heartBubble.SetActive(true);
            startTime = Time.time; // Reset start time when bubble is shown
        }

        if (Time.time > startTime + 1.0f)
        {
            ExitState();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        heartBubble.SetActive(false);
    }
}
