using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Random Tile", menuName = "2D/Tiles/Custom Random Tile")]
public class MyRandomTile : Tile
{
    [Header("Number of frames:")]
    public Sprite[] randomSprites = new Sprite[4];

    public float animationSpeed = 2f;

    public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
        if (randomSprites != null && randomSprites.Length > 0)
        {
            tileAnimationData.animatedSprites = randomSprites;
            tileAnimationData.animationSpeed = animationSpeed;
            tileAnimationData.animationStartTime = Random.Range(0f, 100f);
            return true;
        }
        return false;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        
        if (randomSprites != null && randomSprites.Length > 0 && tileData.sprite == null)
        {
            int index = Random.Range(0,3);
            tileData.sprite = randomSprites[index];
        }
    }
}