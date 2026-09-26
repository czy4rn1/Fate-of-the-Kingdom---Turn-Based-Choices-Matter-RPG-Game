using UnityEngine;

public class SortOrder : MonoBehaviour
{
    public SpriteRenderer playerRenderer;
    public SpriteRenderer sr;
    float playerFeet;
    float treeBase;
    public float offset;

    void Start()
    {
       treeBase = sr.bounds.min.y + offset; 
    }

    void Update()
    {
        playerFeet = playerRenderer.bounds.min.y;
        sr.sortingOrder = playerFeet < treeBase ? 0 : 3;
    }
}
