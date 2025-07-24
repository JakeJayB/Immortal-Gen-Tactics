using UnityEngine;

public class UnitReference : MonoBehaviour {
    public Unit Unit { get; private set; }
    public void Reference(Unit unit) { Unit = unit; }

}
