using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Barrel : MonoBehaviour
{
    private bool isMoved = false;
    private bool isPlayerNearby = false;
    private SpriteRenderer spriteRenderer;
    public GameObject interactIcon;
    public Tilemap decorationsTilemap;
    public Vector3Int currentBarrelPos;
    public Player player;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (interactIcon != null)
        {
            interactIcon.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNearby && !isMoved && Input.GetKeyDown(KeyCode.F) && player.isControllable)
        {
            MoveBarrel();
        }
    }
    void MoveBarrel()
    {
        isMoved = true;
        if (interactIcon != null)
        {
            interactIcon.SetActive(false);
            TileBase barrelTile = decorationsTilemap.GetTile(currentBarrelPos);
            if(barrelTile != null)
            {
                Vector3Int newBarrelPos = new Vector3Int(currentBarrelPos.x - 1, currentBarrelPos.y, currentBarrelPos.z);
                if(decorationsTilemap.GetTile(newBarrelPos) == null)
                {
                    decorationsTilemap.SetTile(currentBarrelPos, null);
                    decorationsTilemap.SetTile(newBarrelPos, barrelTile);
                    currentBarrelPos = newBarrelPos;
                    gameObject.SetActive(false);
                    
                }
            }

        }
        Debug.Log("Player moved the barrel");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isMoved) return;
        if (collision.CompareTag("Player")) {
            isPlayerNearby = true;
            if (interactIcon != null)
            {
                interactIcon.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            isPlayerNearby = false;
            if (interactIcon != null)
            {
                interactIcon.SetActive(false);
            }
        }
    }
}