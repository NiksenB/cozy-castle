using UnityEngine;

public class FollowingNPC : NPC
{
    [SerializeField] private Transform targetToFollow;
    private NPCFollow followBehavior;
    protected override void Start()
    {
        base.Start();
        followBehavior = new NPCFollow(this, animator, heartBubble, transform, myRigidbody, targetToFollow, viewDistance);
        cyclicBehaviors.Add(followBehavior);
        currentBehavior = followBehavior;
    }

    protected override void DiscoverPlayer()
    {
        base.DiscoverPlayer();
        if (targetPlayer == targetToFollow)
        {
            StartCoroutine(ShowBubble(1.0f, exclamationBubble));
            if (currentBehavior != talkBehavior)
            {
                ChangeBehavior(followBehavior);
            }
        }
    }

    protected override void LoseSight()
    {
        base.LoseSight();
        StartCoroutine(ShowBubble(1.0f, exclamationBubble));
        ChangeBehavior(idleBehavior);
    }
}