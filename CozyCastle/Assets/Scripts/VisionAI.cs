using UnityEngine;

public class VisionAI : MonoBehaviour
{
    protected Transform targetPlayer;

    [Header("Perception")]
    public float viewDistance = 5f;
    public void SetTarget(Transform t) => targetPlayer = t;
    public void ClearTarget()
    {
        if (hasEyesOnPlayer)
        {
            LoseSight();
            hasEyesOnPlayer = false;
        }
        targetPlayer = null;
    }

    protected bool hasEyesOnPlayer = false;
    protected enum FacingDirection { Up, Down, Left, Right }
    protected FacingDirection facingDirection = FacingDirection.Down;

    private void Update()
    {
        if (targetPlayer == null) return;

        float dist = Vector2.Distance(transform.position, targetPlayer.position);
        if (dist > viewDistance)
        {
            ClearTarget();
            return;
        }

        if (IsPlayerVisible())
        {
            if (!hasEyesOnPlayer)
            {
                hasEyesOnPlayer = true;
                DiscoverPlayer();
            }
            Vector2 toPlayer = (targetPlayer.position - transform.position).normalized;
            Debug.DrawRay(transform.position, toPlayer * viewDistance, Color.green);
        }
        else
        {
            if (hasEyesOnPlayer)
            {
                ClearTarget();
                LoseSight();
            }
        }
    }

    public bool HasEyesOnPlayer() => hasEyesOnPlayer;

    private void FixedUpdate()
    {
        if (hasEyesOnPlayer)
        {
            // Debug.Log($"{name} has eyes on the player");
            PlayerInVisibilityRange();
        }
    }

    protected virtual void DiscoverPlayer() 
    {
        Debug.Log($"{name} has discovered the player!");
    }

    protected virtual void PlayerInVisibilityRange() 
    {    
        Debug.Log($"{name} is seeing the player!");
    }
    
    protected virtual void LoseSight() {
        Debug.Log($"{name} lost sight of the player.");
    }

    protected virtual bool IsAlert() => true;

    public bool IsPlayerVisible()
    {
        if (targetPlayer == null)
        {
            Debug.LogWarning("Player position is not set. Cannot check visibility.");
            return false;
        }

        if (!IsAlert()) return false;

        float distanceToPlayer = Vector2.Distance(transform.position, targetPlayer.position);
        if (distanceToPlayer > viewDistance) return false;

        if (hasEyesOnPlayer) return true;

        Vector2 facingVector = facingDirection switch
        {
            FacingDirection.Up    => Vector2.up,
            FacingDirection.Down  => Vector2.down,
            FacingDirection.Left  => Vector2.left,
            FacingDirection.Right => Vector2.right,
            _                     => Vector2.zero
        };

        Vector2 toPlayer = (targetPlayer.position - transform.position).normalized;
        if (Vector2.Angle(facingVector, toPlayer) >= 60.0f) return false;

        return true;
    }
}
