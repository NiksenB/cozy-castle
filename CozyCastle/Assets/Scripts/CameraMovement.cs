using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;

    Vector3 cameraOffset;

    void Start()
    {
        cameraOffset = transform.position - target.position;
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z); // TODO
    }

    private void FixedUpdate()
    {
        transform.position = target.position + cameraOffset;
    }
}
