using UnityEngine;

public class Goat : Creature {

    private Rigidbody2D myRigidbody;
    public Animator animator;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
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
