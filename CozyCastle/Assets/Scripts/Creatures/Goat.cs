using UnityEngine;

public class Goat : Creature
{

    private Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        if (myRigidbody == null) Debug.LogError("Rigidbody2D component not found on the Goat object.");

        animator = GetComponentInChildren<Animator>();
        if (animator == null) Debug.LogError("Animator component not found on the Goat object.");
        else SetAnimator(animator);

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerPosition = player.transform;
        }
        else
        {
            Debug.LogWarning("Player object not found. Ensure the player has the 'Player' tag.");
            playerPosition = null;
        }
    }

    void FixedUpdate()
    {
        if (playerPosition != null)
        {
            bool isPlayerVisible = IsPlayerVisible();
            UpdatePlayerVisibility(isPlayerVisible);
            if (isPlayerVisible)
            {
                ReactToPlayer(myRigidbody);
            }
        }
    }
}
