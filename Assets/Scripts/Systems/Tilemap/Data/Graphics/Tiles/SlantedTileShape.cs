using UnityEngine;

public class SlantedTileShape : ITileShape {
    public int GetFaceCount() { return 5; }
    
    public Vector3[] GetVertices() {
        float w = TileProperties.TILE_WIDTH;
        float l = TileProperties.TILE_LENGTH;
        float h = TileProperties.TILE_HEIGHT;

        return new Vector3[] {
            // Back face
            new Vector3(0, 0, 0), new Vector3(w, 0, 0),
            new Vector3(w, h, 0), new Vector3(0, h, 0),
            // Top face
            new Vector3(0, h, 0), new Vector3(w, h, 0),
            new Vector3(w, 0, l), new Vector3(0, 0, l),
            // Bottom face
            new Vector3(0, 0, 0), new Vector3(w, 0, 0),
            new Vector3(w, 0, l), new Vector3(0, 0, l),
            // Left face
            new Vector3(0, 0, 0), new Vector3(0, 0, l),
            new Vector3(0, h, 0),
            // Right face
            new Vector3(w, 0, 0), new Vector3(w, 0, l),
            new Vector3(w, h, 0)
        };
    }

    public int[][] GetTriangles() {
        return new[] {
            // Back face
            new[] { 0, 2, 1, 0, 3, 2 },
            // Top face
            new[] { 4, 6, 5, 4, 7, 6 },
            // Bottom face
            new[] { 8, 9, 10, 8, 10, 11 },
            // Left face
            new[] { 12, 13, 14 },
            // Right face
            new[] { 15, 17, 16 }
        };
    }

    public Vector2[] GetUVMapping() {
        return new[] {
            // Back face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 0.25f), new Vector2(0, 0.25f),
            // Top face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Bottom face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Left face (triangle)
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 0.25f),
            // Right face (triangle)
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 0.25f)
        };
    }
}
