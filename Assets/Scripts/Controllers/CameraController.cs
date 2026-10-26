using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset;
    public float damping;

    public BoxCollider2D leftBorder;
    public BoxCollider2D rightBorder;
    public BoxCollider2D topBorder;
    public BoxCollider2D bottomBorder;
    private float minX, maxX, minY, maxY;
    public bool followPlayer = true;
    public bool onlyHorizontal = false;

    public Transform player;
    private Player playerScript;
    private Vector3 vel = Vector3.zero;

    void Start()
    {
        playerScript = player.GetComponent<Player>();
        CalculateBorders();
    }

    public void CalculateBorders()
    {
        if (leftBorder != null)   minX = leftBorder.bounds.max.x + 10f;
        if (rightBorder != null)  maxX = rightBorder.bounds.min.x - 10f;
        if (bottomBorder != null) minY = bottomBorder.bounds.max.y + 5f;
        if (topBorder != null)    maxY = topBorder.bounds.min.y;
    }

    public void UpdateBorder(BoxCollider2D collider, byte x) {
        switch (x)
        {
            case 0:
                bottomBorder = collider;
                break;
            case 1:
                topBorder = collider;
                break;
            case 2:
                leftBorder = collider;
                break;
            case 3: 
                rightBorder = collider;
                break;            
        }
        CalculateBorders();
    }

    private void FixedUpdate()
    {
        if (followPlayer) {
            Vector3 targetPosition = player.position + offset;
            targetPosition.z = transform.position.z;
            if (onlyHorizontal) targetPosition.x = player.position.x - 4f;
            if (playerScript != null && playerScript.enableClimb) {
                targetPosition.y = playerScript.movement.y >= 0f ? player.position.y + 4.5f : player.position.y -4.5f;
            }
            float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(targetPosition.y, minY, maxY);
            Vector3 clampedPosition = new Vector3(clampedX, clampedY, targetPosition.z);
            transform.position = Vector3.SmoothDamp(transform.position, clampedPosition, ref vel, damping);
        }
    }
    public void ActivateFollowing(bool x)
    {
        followPlayer = x;
    }

}
