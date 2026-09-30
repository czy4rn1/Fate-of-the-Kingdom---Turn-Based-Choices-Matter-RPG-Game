using UnityEngine;

public class SortOrder : MonoBehaviour
{
    public SpriteRenderer sr;
    public float offset;
    public bool isStatic = true;

    void Start()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        UpdateOrder(); 
    }

    void LateUpdate()
    {
        if (!isStatic) UpdateOrder();
    }

    void UpdateOrder()
    {
        float baseY = sr.bounds.min.y + offset;
        sr.sortingOrder = -Mathf.RoundToInt(baseY * 100);
    }
}
