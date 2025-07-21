using System;
using System.Collections.Generic;

[Serializable]
public class TilemapData {
    public List<TileData> tiles = new List<TileData>();
    public List<UnitData> units = new List<UnitData>();
}
