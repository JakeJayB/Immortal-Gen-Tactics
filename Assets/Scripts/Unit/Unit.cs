using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit
{
    public GameObject gameObj { get; set; }
    public UnitInfo unitInfo { get; set; }

    public Unit(GameObject gameObj)
    {
        this.gameObj = gameObj;
        unitInfo = gameObj.GetComponent<UnitInfo>();
        unitInfo.unit = this;
    }

    public Unit Initialize(Vector3Int initLocation, UnitDirection unitDirection)
    {
        unitInfo.CellLocation = initLocation;
        unitInfo.UnitDirection = unitDirection;
        unitInfo.sprite = Resources.Load<Sprite>("Sprites/Units/Test_Player/Test_Sprite(Right)");

        SpriteRenderer spriteRender = gameObj.GetComponent<SpriteRenderer>();
        UnitRenderer unitRenderer = new UnitRenderer(spriteRender);
        unitRenderer.Render(initLocation, unitDirection);

        gameObj.AddComponent<BillboardEffect>();
        return this;
    }
}
