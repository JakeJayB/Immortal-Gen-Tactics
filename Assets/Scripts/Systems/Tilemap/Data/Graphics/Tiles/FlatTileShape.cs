using UnityEngine;

public class FlatTileShape : ITileShape {
    public int GetFaceCount() { return 6; }
    
    public Vector3[] GetVertices() {
        float w = TileProperties.TILE_WIDTH;
        float l = TileProperties.TILE_LENGTH;
        float h = TileProperties.TILE_HEIGHT;

        return new[] {
            // Front face
            new Vector3(0, 0, l), new Vector3(w, 0, l),
            new Vector3(w, h, l), new Vector3(0, h, l),
            // Back face
            new Vector3(0, 0, 0), new Vector3(w, 0, 0),
            new Vector3(w, h, 0), new Vector3(0, h, 0),
            // Top face
            new Vector3(0, h, 0), new Vector3(w, h, 0),
            new Vector3(w, h, l), new Vector3(0, h, l),
            // Bottom face
            new Vector3(0, 0, 0), new Vector3(w, 0, 0),
            new Vector3(w, 0, l), new Vector3(0, 0, l),
            // Left face
            new Vector3(0, 0, 0), new Vector3(0, 0, l),
            new Vector3(0, h, l), new Vector3(0, h, 0),
            // Right face
            new Vector3(w, 0, 0), new Vector3(w, 0, l),
            new Vector3(w, h, l), new Vector3(w, h, 0)
        };
    }

    public int[][] GetTriangles() {
        return new[] {
            // Front face
            new[] { 0, 1, 2, 0, 2, 3 },
            // Back face
            new[] { 4, 6, 5, 4, 7, 6 },
            // Top face
            new[] { 8, 10, 9, 8, 11, 10 },
            // Bottom face
            new[] { 12, 13, 14, 12, 14, 15 },
            // Left face
            new[] { 16, 17, 18, 16, 18, 19 },
            // Right face
            new[] { 20, 22, 21, 20, 23, 22 }
        };
    }

    public Vector2[] GetUVMapping() {
        return new[] {
            // Front face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 0.25f), new Vector2(0, 0.25f),
            // Back face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 0.25f), new Vector2(0, 0.25f),
            // Top face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Bottom face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Left face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 0.25f), new Vector2(0, 0.25f),
            // Right face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 0.25f), new Vector2(0, 0.25f)
        };
    }
}
