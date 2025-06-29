using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{

    public static List<Unit> unitList;

    private void Awake()
    {
        unitList = new List<Unit>();
        for (int i = 0; i < 5; i++)
        {
            Unit unit = Unit.Initialize(Vector3Int.zero, UnitDirection.Forward);
            unit.transform.SetParent(transform);
            unit.gameObj.SetActive(false);
            unitList.Add(unit);
        }       
    }


    public static void Clear()
    {
        unitList = null;
    }

    public static void RegisterCleanup()
    {
        MemoryManager.AddListeners(Clear);
    }
}
