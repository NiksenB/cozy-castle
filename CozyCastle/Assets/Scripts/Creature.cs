using UnityEngine;

public enum CreatureState
{
    Idle,
    Moving,
    Interacting,
    Reacting,
    Sleeping,
}

public class Creature : MonoBehaviour
{
    public string creatureName = "Default Creature";
    public bool isFriendly = true;
    public bool hasEyesOnPlayer = false;
    public CreatureState currentState = CreatureState.Idle;
    public int affectionLevel = 0;
    public float speed = 4.0f;
    public float interactionRadius = 0.8f;
    public float visibilityRange = 5.0f;
    public Transform playerPosition;

    public void Interact()
    {
        if (IsPlayerInInteractionRange())
        {
            ChangeState(CreatureState.Interacting);
            affectionLevel++;
            Debug.Log("Creature interacted with. Affection level: " + affectionLevel);
        }
        else
        {
            Debug.Log("Player is too far away to interact with the creature.");
        }
    }

    public bool IsPlayerInInteractionRange()
    {
        if (playerPosition != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);
            bool isInRange = distanceToPlayer <= interactionRadius;
            Debug.Log(creatureName + " interaction check: " + (isInRange ? "Player is in range." : "Player is out of range."));
            return isInRange;
        }
        return false;
    }

    public bool IsPlayerVisible()
    {
        if (playerPosition != null && currentState != CreatureState.Sleeping)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);
            return
                distanceToPlayer <= visibilityRange &&
                Vector2.Angle(transform.up, playerPosition.position - transform.position) < 60.0f;
        }
        Debug.LogWarning("Player position is not set. Cannot check visibility.");
        return false;
    }

    public void MoveTowardsPlayer(Rigidbody2D myRigidbody)
    {
        if (playerPosition != null)
        {
            if (currentState != CreatureState.Moving)
            {
                ChangeState(CreatureState.Moving);
                Debug.Log(creatureName + " is now moving towards the player.");
            }
            // Vector3 directionToPlayer = (playerPosition.position - transform.position).normalized;
            // transform.position += speed * Time.deltaTime * directionToPlayer;

            Vector2 temp = Vector2.MoveTowards(transform.position, playerPosition.position, speed * Time.deltaTime);
            myRigidbody.MovePosition(temp);
        }
        else
        {
            Debug.Log("Player position is not set. Cannot move towards player.");
        }
    }

    public void UpdatePlayerVisibility(bool isPlayerVisible = true)
    {
        if (isPlayerVisible && !hasEyesOnPlayer)
        {
            Debug.Log(creatureName + " has spotted the player.");
            ChangeState(CreatureState.Reacting);
            hasEyesOnPlayer = true;
        }
        else if (!isPlayerVisible && hasEyesOnPlayer)
        {
            Debug.Log(creatureName + " has lost sight of the player.");
            ChangeState(CreatureState.Idle);
            hasEyesOnPlayer = false;
        }
    }

    public void ReactToPlayer(Rigidbody2D myRigidbody)
    {
        if (!hasEyesOnPlayer)
        {
            hasEyesOnPlayer = true;
            Debug.Log(creatureName + " has eyes on the player.");
            ChangeState(CreatureState.Reacting);
        }

        Debug.Log(creatureName + " is reacting to the player.");
        if (isFriendly)
        {
            if (!IsPlayerInInteractionRange())
            {
                MoveTowardsPlayer(myRigidbody);
            }
            else
            {
                ChangeState(CreatureState.Idle);
            }
        }
        else
        {
            Debug.Log(creatureName + " is not friendly and does not approach the player.");
        }
    }

    public void ChangeState(CreatureState newState)
    {
        if (currentState == newState)
        {
            return;
        }

        if (newState == CreatureState.Sleeping)
        {
            hasEyesOnPlayer = false;
        }

        currentState = newState;
        Debug.Log(creatureName + " changed state to: " + currentState);
    }
}
