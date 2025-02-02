using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRenderer : MonoBehaviour
{
    private const float HEIGHT_OFFSET = TileProperties.TILE_HEIGHT + 0.05f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // TODO: Find a way to work with sprite sheets instead of individual sprite for efficiency
    public void Render(Vector3Int cellLocation)
    {
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite =
            Resources.Load<Sprite>("Sprites/Units/Test_Player/Test_Sprite(Down)");
        
        PositionUnit(cellLocation);
    }

    private void PositionUnit(Vector3Int cellLocation)
    {
        gameObject.transform.position = new Vector3(
            cellLocation.x * TileProperties.TILE_WIDTH,
            cellLocation.y * TileProperties.TILE_HEIGHT + HEIGHT_OFFSET, 
            cellLocation.z * TileProperties.TILE_LENGTH);
    }
}
