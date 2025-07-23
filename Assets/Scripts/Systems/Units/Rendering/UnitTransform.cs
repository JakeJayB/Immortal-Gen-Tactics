using System;
using UnityEngine;

public class UnitTransform {
    public static void RotateUnit(UnitRenderer unitRenderer, UnitDirection unitDirection) {
        switch (unitDirection) {
            case UnitDirection.Forward:
                unitRenderer.Transform().Rotate(0, 0, 0);
                break;
            case UnitDirection.Backward:
                unitRenderer.Transform().Rotate(0, 180, 0);
                break;
            case UnitDirection.Left:
                unitRenderer.Transform().Rotate(0, 90, 0);
                break;
            case UnitDirection.Right:
                unitRenderer.Transform().Rotate(0, 270, 0);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(unitDirection), unitDirection, null);
        }
    }

    public static void PositionUnit(UnitRenderer unitRenderer, Vector3Int cellLocation) {
        unitRenderer.Transform().position = new Vector3(
            cellLocation.x * TileProperties.TILE_WIDTH,
            cellLocation.y * TileProperties.TILE_HEIGHT + UnitRenderer.HEIGHT_OFFSET, 
            cellLocation.z * TileProperties.TILE_LENGTH);
    }
}
