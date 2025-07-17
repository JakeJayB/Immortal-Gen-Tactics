using System;
using UnityEngine;

public static class TileProperties {
    public const float TILE_WIDTH = 0.5f;
    public const float TILE_LENGTH = 0.5f;
    public const float TILE_HEIGHT = 0.25f;
    
    public static ITileShape GetProperties(TileType tileType) {
        return tileType switch {
            TileType.Flat => new FlatTileShape(),
            TileType.Slanted => new SlantedTileShape(),
            TileType.Slanted_Corner => new SlantedCornerTileShape(),
            TileType.Stairs => new StairsTileShape(),
            _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
        };
    }
}
