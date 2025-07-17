using System.Collections.Generic;

public class OverlayTileHashset {
    private static readonly Dictionary<TileType, HashSet<int>> TileSides = new()
    {
        { TileType.Flat, new HashSet<int> {2} },
        { TileType.Slanted, new HashSet<int> {1} },
        { TileType.Slanted_Corner, new HashSet<int> {1, 2} },
        { TileType.Stairs, new HashSet<int> {0, 4, 8} }
    };
    
    public static HashSet<int> GetHashset(TileType tileType) {
        return TileSides.TryGetValue(tileType, out var result) ? result : new HashSet<int>();
    }
}
