using System.Collections;
using UnityEngine;

public abstract class Item : UnitAction {
    private UnitAction Storage = null;
    protected void AccessStorage(UnitAction storage) { Storage = storage; }
    protected IEnumerator Consume(Unit unit, Vector2Int selectedCell) {
        // Consume Item and Remove it from ActionSet
        if (Storage != null) {
            yield return Storage.ExecuteAction(unit, selectedCell);
        } else {
            unit.ActionSet.RemoveAction(this);
        }
    }
}
