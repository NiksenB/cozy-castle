using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;

    // Called when the game starts
    private void Awake()
    {
        inventory = new Inventory(24);
    }
}
