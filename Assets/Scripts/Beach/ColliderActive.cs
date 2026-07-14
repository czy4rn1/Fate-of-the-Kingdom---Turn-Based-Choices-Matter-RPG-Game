using UnityEngine;

public class ColliderActive : MonoBehaviour
{
    
    public BoxCollider2D col1;
    public BoxCollider2D col2;
    public Player player;
    public CameraController cameraController;

    void Update()
    {
        if (player != null)
        {
            if (player.transform.position.x < 32.5)
            {
                col1.enabled = true;
                col2.enabled = false;
                cameraController.UpdateBorder(col1, 0);
            }
            else
            {
                col2.enabled = true;
                col1.enabled = false;
                cameraController.UpdateBorder(col2, 0);
            }
        }
    }
}
