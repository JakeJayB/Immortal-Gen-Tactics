using UnityEngine;

public class Tile {
    public GameObject TileObj { get; private set; }
    public TileInfo TileInfo { get; private set; }
    public OverlayTile OverlayTile { get; set; }
    
    public Tile(Vector3Int cellLocation, TileType tileType, TerrainType terrainType, TileDirection direction, bool isStartArea, bool isTraversable, GameObject OverlayObjPrefab = null) {
        TileObj = new GameObject("Tile: " + cellLocation);
        TileObj.AddComponent<TileReference>().Reference(this);
        TileInfo = new TileInfo(cellLocation, tileType, terrainType, direction, isStartArea, isTraversable);
        TileRenderer.Render(this, TilePrefabLibrary.FindPrefabPath(this));
        TryAddOverlayTile(OverlayObjPrefab);
    }

    private void TryAddOverlayTile(GameObject OverlayObjPrefab) {
        if (OverlayObjPrefab) OverlayTile = new OverlayTile(this, OverlayObjPrefab);
    }
    
    public bool IsSelectable() { return OverlayTile.OverlayObj.activeSelf; }
}