using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRenderer : MonoBehaviour
{
    private const float HEIGHT_OFFSET = 0.05f;
    
    
    // TODO: Find a way to work with sprite sheets instead of individual sprite for efficiency
    public void Render(Vector3Int cellLocation, UnitDirection unitDirection)
    {
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite =
            Resources.Load<Sprite>("Sprites/Units/Test_Player/Test_Sprite(Down)");
        
        PositionUnit(cellLocation);
        RotateUnit(unitDirection);
    }


    public void RotateUnit(UnitDirection unitDirection)
    {
        switch (unitDirection)
        {
            case UnitDirection.Forward:
                gameObject.transform.Rotate(0, 0, 0);
                break;
            case UnitDirection.Backward:
                gameObject.transform.Rotate(0, 180, 0);
                break;
            case UnitDirection.Left:
                gameObject.transform.Rotate(0, 90, 0);
                break;
            case UnitDirection.Right:
                gameObject.transform.Rotate(0, 270, 0);
                break;
        }
    }

    public void PositionUnit(Vector3Int cellLocation)
    {
        gameObject.transform.position = new Vector3(
            cellLocation.x * TileProperties.TILE_WIDTH,
            cellLocation.y * TileProperties.TILE_HEIGHT + HEIGHT_OFFSET, 
            cellLocation.z * TileProperties.TILE_LENGTH);
    }
}
