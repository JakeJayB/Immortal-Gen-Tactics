using System;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTilePrefabLibrary {
    private static readonly string OverlayFlatPrefab = "Prefabs/Tilemap/Tiles/Overlay/Flat";
    private static readonly string OverlaySlantedPrefab = "Prefabs/Tilemap/Tiles/Overlay/Slanted";
    private static readonly string OverlaySlantedCornerPrefab = "Prefabs/Tilemap/Tiles/Overlay/Slanted_Corner";
    private static readonly string OverlayStairsPrefab = "Prefabs/Tilemap/Tiles/Overlay/Stairs";

    public static GameObject FindPrefab(TileType tileType) {
        return tileType switch {
            TileType.Flat => Resources.Load<GameObject>(OverlayFlatPrefab),
            TileType.Slanted => Resources.Load<GameObject>(OverlaySlantedPrefab),
            TileType.Slanted_Corner => Resources.Load<GameObject>(OverlaySlantedCornerPrefab),
            TileType.Stairs => Resources.Load<GameObject>(OverlayStairsPrefab),
            _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
        };
    }
}
