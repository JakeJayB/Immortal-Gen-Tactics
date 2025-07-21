using System;
using UnityEngine;

[Serializable]
public class UnitData {
    public Vector3Int cellLocation;
    public UnitDirection unitDirection;

    public UnitData(Vector3Int cellLocation, UnitDirection unitDirection) {
        this.cellLocation = cellLocation;
        this.unitDirection = unitDirection;
    }
}
