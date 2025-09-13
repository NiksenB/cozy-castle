using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class Flee : AnimalState
{
    private Transform targetPlayer;
    private Transform transform;
    private Rigidbody2D rigidbody;
    private GameObject exclamationBubble;
    private float speed;
    private float safeDistance = 20.0f;
    private float panickedRedirectTime = 0.0f;
    private float despawnTime = 7.0f;
    private Vector3 panickedDirection; 

    public Flee(Animal animal, Animator animator, GameObject exclamationBubble, Transform transform, Rigidbody2D rigidbody, float speed) : base(animal, animator)
    {
        this.exclamationBubble = exclamationBubble;
        this.transform = transform;
        this.rigidbody = rigidbody;
        this.speed = speed;
    }

    public override void EnterState()
    {
        base.EnterState();
        animator.SetBool("isMoving", false);
        exclamationBubble.SetActive(true);
        animator.SetTrigger("react");
    }

    public override void UpdateState()
    {
        if (isExitingState) return;
        base.UpdateState();
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Reaction"))
        {
            return; // Wait until the "Reaction" animation is finished
        }
        animator.SetBool("isMoving", true);
        exclamationBubble.SetActive(false);
        if (Time.time > startTime + despawnTime)
        {
            Debug.Log("Despawn time reached, exiting Flee state.");
            ExitState();
            return;
        }

        if (Vector3.Distance(transform.position, targetPlayer.position) > safeDistance)
        {
            Debug.Log("Safe distance reached, exiting Flee state.");
            ExitState();
            return;
        }
        
        if (Time.time - panickedRedirectTime < 1f)
        {
            Vector3 newPosition = transform.position + 1.8f * speed * Time.deltaTime * panickedDirection.normalized;
            rigidbody.MovePosition(newPosition);
            animal.ChangeAnim(panickedDirection);
            return;
        }
        MoveAwayFromPlayer();

    }

    public override void ExitState()
    {
        base.ExitState();
        isExitingState = true;
        animator.SetBool("isMoving", false);
        exclamationBubble.SetActive(false);
        GameObject.Destroy(animal.gameObject, 0.5f); // Delay destruction to allow exit animations to play
    }

    private void MoveAwayFromPlayer()
    {
        Vector3 directionAwayFromPlayer = transform.position - targetPlayer.position;
        directionAwayFromPlayer.Normalize();
        Vector3 newPosition = transform.position + 1.5f * speed * Time.deltaTime * directionAwayFromPlayer;
        rigidbody.MovePosition(newPosition);
        animal.ChangeAnim(directionAwayFromPlayer);
    }

    public void SetTargetPlayer(Transform targetPlayer)
    {
        this.targetPlayer = targetPlayer;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        panickedRedirectTime = Time.time;

        // rotate 100 degrees to the right
        Vector3 dir = transform.position - targetPlayer.position;
        panickedDirection = Quaternion.Euler(0, 0, 100) * dir;
    }
}