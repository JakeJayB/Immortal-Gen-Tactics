using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public GameObject gameObj { get; private set; }

    public UnitInfo unitInfo { get; set; }

    public UnitEquipment unitEquipment { get; set; }

    public void Initialize(Vector3Int initLocation)
    {
        GameObject gameObj = new GameObject("Unit " + initLocation);
        unitInfo = gameObj.AddComponent<UnitInfo>();
        unitInfo.CellLocation = initLocation;

        unitEquipment = gameObj.AddComponent<UnitEquipment>();

        UnitRenderer unitRenderer = gameObj.AddComponent<UnitRenderer>();
        unitRenderer.Render(initLocation);
        
    }
}
