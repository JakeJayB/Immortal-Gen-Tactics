using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{

    List<Unit> unitList = new List<Unit>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            Unit unit = Unit.Initialize(Vector3Int.zero, UnitDirection.Forward);
            unit.transform.SetParent(transform);
            unit.gameObj.SetActive(false);  
            unitList.Add(unit);
        }
    }

}
