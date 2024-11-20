using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Player player;

    void LateUpdate()
    {
        transform.position = player.transform.position + new Vector3(0, 0, -10);
    }
}
