using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit
{
    public GameObject gameObj { get; set; }
    
    // Unit Properties
    public UnitInfo unitInfo { get; protected set; }
    public UnitEquipment equipment { get; protected set; }
    public UnitActionSet ActionSet { get; protected set; }

    // Constructor
    public Unit(GameObject gameObj, UnitDefinitionData unitData)
    {
        this.gameObj = gameObj;

        if (unitData == null) {
            Debug.LogError($"[{gameObj.name}]: UDD not found!");
        }
        
        unitInfo = new UnitInfo(this);
        unitInfo.Initialize(unitData);
        
        ActionSet = new UnitActionSet(this);
        ActionSet.Initialize(unitData);
        
        equipment = new UnitEquipment(this);
        equipment.Initialize(unitData);
        
        unitInfo.ResetCurrentStatPoints();
    }

    public Unit Initialize(Vector3Int initLocation, UnitDirection unitDirection)
    {
        unitInfo.CellLocation = initLocation;
        unitInfo.UnitDirection = unitDirection;
        unitInfo.sprite = Resources.Load<Sprite>("Sprites/Units/Test_Player/Test_Sprite(Right)");

        SpriteRenderer spriteRender = gameObj.GetComponent<SpriteRenderer>();
        UnitRenderer unitRenderer = new UnitRenderer(spriteRender);
        unitRenderer.Render(initLocation, unitDirection);
        
        return this;
    }
}
