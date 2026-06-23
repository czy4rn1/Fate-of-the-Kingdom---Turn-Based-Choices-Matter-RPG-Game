using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;

    public Transform player;
    private Vector3 vel = Vector3.zero;
    private void FixedUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        targetPosition.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, damping);
    }

}
