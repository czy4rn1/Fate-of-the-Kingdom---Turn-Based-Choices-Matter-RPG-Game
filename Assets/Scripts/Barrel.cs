using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Barrel : MonoBehaviour
{
    private bool isMoved = false;
    public Tilemap decorationsTilemap;
    public Vector3Int currentBarrelPos;
    public Player player;
    public PlayerDetection playerDetection;
    void Update()
    {
        if (playerDetection != null) {
            playerDetection.allowIcon = PlayerData.Instance.strength > 10;
            if (playerDetection.isPlayerNearby && !isMoved && Input.GetKeyDown(KeyCode.F) && player.isControllable)
            {
                if (PlayerData.Instance.strength > 10) MoveBarrel();
            }
        }
    }
    void MoveBarrel()
    {
        isMoved = true;
        if (playerDetection.interactIcon != null)
        {
            playerDetection.interactIcon.SetActive(false);
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
}