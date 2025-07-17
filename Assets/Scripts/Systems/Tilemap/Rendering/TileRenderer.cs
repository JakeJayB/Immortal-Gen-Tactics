using UnityEngine;

public class TileRenderer {
    public static void Render(Tile tile, string prefabLocation) {
        if (tile == null || !tile.TileObj || tile.TileInfo == null) {
            Debug.LogError("[TileRenderer]: Tile Is Not Fully Defined");
            return;
        }

        GameObject tileObj = tile.TileObj;
        TileInfo tileInfo = tile.TileInfo;
        
        // Access properties of the tile's type
        ITileShape tileProperties = TileProperties.GetProperties(tileInfo.TileType);
        
        // Create a new mesh for the tile
        Mesh mesh = new Mesh {
            vertices = tileProperties.GetVertices(),
            uv = tileProperties.GetUVMapping()
        };

        // Center the pivots of each tile
        mesh.vertices = TileTransform.CenterPivots(mesh.vertices);

        // Triangles for each face
        int[][] triangles = tileProperties.GetTriangles();

        // Assign triangles to sub-meshes
        mesh.subMeshCount = tileProperties.GetFaceCount();
        for (int i = 0; i < mesh.subMeshCount; i++) {
            mesh.SetTriangles(triangles[i], i);
        }

        mesh.RecalculateNormals();

        MeshFilter meshFilter = tileObj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = tileObj.AddComponent<MeshRenderer>();
        meshRenderer.materials = Terrain.GetTerrain(prefabLocation);
 
        TileTransform.PositionTile(tileObj, tileInfo.CellLocation);
        TileTransform.RotateTile(tileObj, tileInfo.TileDirection);
    }
}
