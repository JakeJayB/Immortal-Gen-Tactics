using UnityEngine;

public class UnitRenderer
{
    private const float HEIGHT_OFFSET = 0.05f;
    private readonly Unit Unit;
    private readonly SpriteRenderer SpriteRenderer;
    private readonly Transform Transform;
    
    public UnitRenderer(Unit unit, SpriteRenderer spriteRenderer) {
        Unit = unit;
        Unit.GameObj.AddComponent<BillboardEffect>();
        SpriteRenderer = spriteRenderer;
        Transform = spriteRenderer.transform;
    }
    
    // TODO: Find a way to work with sprite sheets instead of individual sprite for efficiency
    public void Render(Sprite sprite) {
        SpriteRenderer.sprite = sprite;
        PositionUnit(Unit.UnitInfo.CellLocation);
        RotateUnit(Unit.UnitInfo.UnitDirection);
    }


    public void RotateUnit(UnitDirection unitDirection)
    {
        switch (unitDirection)
        {
            case UnitDirection.Forward:
                Transform.Rotate(0, 0, 0);
                break;
            case UnitDirection.Backward:
                Transform.Rotate(0, 180, 0);
                break;
            case UnitDirection.Left:
                Transform.Rotate(0, 90, 0);
                break;
            case UnitDirection.Right:
                Transform.Rotate(0, 270, 0);
                break;
        }
    }

    public void PositionUnit(Vector3Int cellLocation)
    {
        Transform.position = new Vector3(
            cellLocation.x * TileProperties.TILE_WIDTH,
            cellLocation.y * TileProperties.TILE_HEIGHT + HEIGHT_OFFSET, 
            cellLocation.z * TileProperties.TILE_LENGTH);
    }

    public Sprite GetSprite() { return SpriteRenderer.sprite; }
}
