using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject gameObj { get; set; }

    public UnitInfo unitInfo { get; set; }

    public UnitRenderer unitRenderer { get; set; }
    public UnitMovement unitMovement { get; set; }

    public static Unit Initialize(Vector3Int initLocation, UnitDirection unitDirection)
    {
        GameObject gameObj = Instantiate(Resources.Load<GameObject>("Prefabs/Unit/Unit"));
        Unit unit = gameObj.AddComponent<Unit>();
        unit.gameObj = gameObj;

        unit.unitInfo = gameObj.GetComponent<UnitInfo>();
        unit.unitInfo.CellLocation = initLocation;
        unit.unitInfo.UnitDirection = unitDirection;
        unit.unitInfo.sprite = Resources.Load<Sprite>("Sprites/Units/Test_Player/Test_Sprite(Right)");

        unit.unitRenderer = gameObj.AddComponent<UnitRenderer>();
        unit.unitRenderer.Render(initLocation, unitDirection);

        unit.gameObj.AddComponent<BillboardEffect>();
        unit.unitMovement = unit.gameObj.AddComponent<UnitMovement>();
        return unit;
    }
}
