using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : UnitAction
{
    private UnitAction Storage = null;
    protected void Store(UnitAction storage) { Storage = storage; }
    protected IEnumerator Consume(Unit unit, Vector2Int selectedCell)
    {
        // Consume Item and Remove it from ActionSet
        if (Storage != null) {
            yield return Storage.ExecuteAction(unit, selectedCell);
        } else {
            unit.unitInfo.ActionSet.RemoveAction(this);
        }
    }
}
