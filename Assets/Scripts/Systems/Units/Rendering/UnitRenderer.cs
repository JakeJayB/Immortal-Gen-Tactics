using UnityEngine;

public class UnitRenderer {
    public const float HEIGHT_OFFSET = 0.05f;
    private readonly Unit Unit;
    private readonly SpriteRenderer SpriteRenderer;
    
    public UnitRenderer(Unit unit, SpriteRenderer spriteRenderer) {
        Unit = unit;
        Unit.GameObj.AddComponent<BillboardEffect>();
        SpriteRenderer = spriteRenderer;
    }
    
    // TODO: Find a way to work with sprite sheets instead of individual sprite for efficiency
    public void Render(Sprite sprite) {
        SpriteRenderer.sprite = sprite;
        UnitTransform.PositionUnit(this, Unit.UnitInfo.CellLocation);
        UnitTransform.RotateUnit(this, Unit.UnitInfo.UnitDirection);
    }

    public Sprite Sprite() { return SpriteRenderer.sprite; }
    public Transform Transform() { return SpriteRenderer.transform; }
}
