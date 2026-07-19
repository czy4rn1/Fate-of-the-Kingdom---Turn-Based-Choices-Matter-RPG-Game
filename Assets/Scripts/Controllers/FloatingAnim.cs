using UnityEngine;

public class FloatingAnim : MonoBehaviour
{
    public float jumpRange = 0.1f;
    public float speed = 5f;
    public bool horizontal = false;

    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        float sin = Mathf.Sin(Time.time * speed);
        float offset = sin * jumpRange;
        if (!horizontal) transform.position = new Vector3(startPosition.x, startPosition.y + offset, startPosition.z);
        else transform.position = new Vector3(startPosition.x + offset, startPosition.y, startPosition.z);
    }
}
