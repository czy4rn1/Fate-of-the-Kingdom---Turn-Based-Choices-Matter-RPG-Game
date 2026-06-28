using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;

    public BoxCollider2D leftBorder;
    public BoxCollider2D rightBorder;
    public BoxCollider2D topBorder;
    public BoxCollider2D bottomBorder;
    private float minX, maxX, minY, maxY;

    public Transform player;
    private Vector3 vel = Vector3.zero;
    Vector3 curPosition;

    void Start()
    {
        if (leftBorder != null)   minX = leftBorder.bounds.max.x + 5f;
        if (rightBorder != null)  maxX = rightBorder.bounds.min.x - 5f;
        if (bottomBorder != null) minY = bottomBorder.bounds.max.y + 2f;
        if (topBorder != null)    maxY = topBorder.bounds.min.y;
    }
    private void FixedUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        targetPosition.z = transform.position.z;
        float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPosition.y, minY, maxY);
        Vector3 clampedPosition = new Vector3(clampedX, clampedY, targetPosition.z);
        transform.position = Vector3.SmoothDamp(transform.position, clampedPosition, ref vel, damping);
    }

}
