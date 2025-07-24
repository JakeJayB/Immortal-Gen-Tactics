using UnityEngine;
public class Unit {
    public GameObject GameObj { get; set; }
    public UnitInfo UnitInfo { get; protected set; }
    public UnitEquipment Equipment { get; protected set; }
    public UnitActionSet ActionSet { get; protected set; }
    public UnitRenderer UnitRenderer { get; protected set; }
    
    public Unit(GameObject gameObj, UnitDefinitionData unitData, SpriteRenderer spriteRenderer) {
        GameObj = gameObj;
        GameObj.AddComponent<UnitReference>().Reference(this);

        if (unitData == null) {
            Debug.LogError($"[Unit]: UDD not found for Unit '{gameObj.name}' during construction.");
        }
        
        UnitInfo = new UnitInfo(this);
        UnitInfo.Initialize(unitData);
        
        ActionSet = new UnitActionSet(this);
        ActionSet.Initialize(unitData);
        
        Equipment = new UnitEquipment(this);
        Equipment.Initialize(unitData);
        
        UnitRenderer = new UnitRenderer(this, spriteRenderer);
        
        UnitInfo.ResetCurrentStatPoints();
    }

    public void Initialize(Vector3Int initLocation, UnitDirection unitDirection) {
        SetInitialPosition(initLocation, unitDirection);
        UnitRenderer.Render(Resources.Load<Sprite>("Sprites/Units/Test_Player/Test_Sprite(Right)"));
    }

    protected void SetInitialPosition(Vector3Int initLocation, UnitDirection unitDirection) {
        UnitInfo.CellLocation = initLocation;
        UnitInfo.UnitDirection = unitDirection;
    }
}
