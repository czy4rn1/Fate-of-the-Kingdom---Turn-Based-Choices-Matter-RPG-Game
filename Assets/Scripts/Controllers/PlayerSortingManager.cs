using UnityEngine;

public class PlayerSortingManager : MonoBehaviour
{
    private Renderer playerRenderer;
    private BoxCollider2D boxCollider2D;
    void Awake()
    {
        playerRenderer = GetComponent<Renderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void setSorting(string layerName)
    {
        playerRenderer.sortingLayerName = layerName;
        if (layerName == "Background")
        {
            playerRenderer.sortingOrder = 1;
        }
        else if (layerName == "Characters")
        {
            playerRenderer.sortingOrder = 0;
        }
    }

    public void setCollision(bool x)
    {
        if (x)
        {
            boxCollider2D.isTrigger = true;
        }
        else
        {
            boxCollider2D.isTrigger = false;
        }
    }
}
