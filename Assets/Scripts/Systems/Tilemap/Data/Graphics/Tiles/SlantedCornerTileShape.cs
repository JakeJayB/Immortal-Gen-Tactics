using UnityEngine;

public class SlantedCornerTileShape : ITileShape {
    public int GetFaceCount() { return 5; }
    
    public Vector3[] GetVertices() {
        float w = TileProperties.TILE_WIDTH;
        float l = TileProperties.TILE_LENGTH;
        float h = TileProperties.TILE_HEIGHT;

        return new Vector3[] {
            // Back face
            new Vector3(0, 0, 0), new Vector3(w, 0, 0), 
            new Vector3(0, h, 0),
            // Top-Left face
            new Vector3(0, h, 0), new Vector3(w, 0, 0), 
            new Vector3(w, 0, l),
            // Top-Right Face
            new Vector3(0, h, 0), new Vector3(0, 0, l), 
            new Vector3(w, 0, l),
            // Bottom face
            new Vector3(0, 0, 0), new Vector3(w, 0, 0), 
            new Vector3(w, 0, l), new Vector3(0, 0, l),
            // Left face
            new Vector3(0, 0, 0), new Vector3(0, 0, l), 
            new Vector3(0, h, 0)
        };
    }
    
    public int[][] GetTriangles() {
        return new[] {
            // Back face
            new[] { 0, 2, 1 },
            // Top-Left face
            new[] { 3, 5, 4 },
            // Top-Right face
            new[] { 6, 7, 8 },
            // Bottom face
            new[] { 9, 10, 11, 9, 11, 12 },
            // Left face
            new[] { 13, 14, 15 }
        };
    }

    public Vector2[] GetUVMapping() {
        return new[] {
            // Back face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 0.25f),
            // Top-Left face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 0.25f),
            // Top-Right face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 0.25f),
            // Bottom face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Left face (triangle)
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 0.25f)
        };
    }
}
