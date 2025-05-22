using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;

    Vector3 cameraOffset;

    void Start()
    {
        cameraOffset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        transform.position = target.position + cameraOffset;
    }
}
