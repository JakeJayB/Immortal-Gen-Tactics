using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRenderer
{
    private const float HEIGHT_OFFSET = 0.05f;
    private readonly SpriteRenderer spriteRenderer;
    private readonly Transform transform;
    
    public UnitRenderer(SpriteRenderer spriteRenderer)
    {
        this.spriteRenderer = spriteRenderer;
        transform = spriteRenderer.transform;
    }
    
    // TODO: Find a way to work with sprite sheets instead of individual sprite for efficiency
    public void Render(Vector3Int cellLocation, UnitDirection unitDirection) {
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Units/Test_Player/Test_Sprite(Down)");
        PositionUnit(cellLocation);
        RotateUnit(unitDirection);
    }


    public void RotateUnit(UnitDirection unitDirection)
    {
        switch (unitDirection)
        {
            case UnitDirection.Forward:
                transform.Rotate(0, 0, 0);
                break;
            case UnitDirection.Backward:
                transform.Rotate(0, 180, 0);
                break;
            case UnitDirection.Left:
                transform.Rotate(0, 90, 0);
                break;
            case UnitDirection.Right:
                transform.Rotate(0, 270, 0);
                break;
        }
    }

    public void PositionUnit(Vector3Int cellLocation)
    {
        transform.position = new Vector3(
            cellLocation.x * TileProperties.TILE_WIDTH,
            cellLocation.y * TileProperties.TILE_HEIGHT + HEIGHT_OFFSET, 
            cellLocation.z * TileProperties.TILE_LENGTH);
    }
}
