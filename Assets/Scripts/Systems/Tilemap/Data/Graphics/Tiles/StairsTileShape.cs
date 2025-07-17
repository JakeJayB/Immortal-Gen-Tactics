using UnityEngine;

public class StairsTileShape : ITileShape {
    public int GetFaceCount() { return 14; }
    
    public Vector3[] GetVertices() {
        float w = TileProperties.TILE_WIDTH;
        float l = TileProperties.TILE_LENGTH;
        float h = TileProperties.TILE_HEIGHT;

        return new Vector3[] {
            // Top Step ------------------------------------------------------------
                
            // Top face
            new Vector3(0, h, 0), new Vector3(w, h, 0), 
            new Vector3(w, h, l / 3), new Vector3(0, h, l / 3),
            // Front face
            new Vector3(w, h, l / 3), new Vector3(0, h, l / 3),
            new Vector3(0, h * 2 / 3, l / 3), new Vector3(w, h * 2 / 3, l / 3),
            // Left face
            new Vector3(0, 0, 0), new Vector3(0, 0, l / 3), 
            new Vector3(0, h, 0), new Vector3(0, h, l / 3),
            // Right face
            new Vector3(w, 0, 0), new Vector3(w, 0, l / 3), 
            new Vector3(w, h, 0), new Vector3(w, h, l / 3),
            
            // Mid Step -------------------------------------------------------------
            
            // Top face
            new Vector3(0, h * 2 / 3, l / 3), new Vector3(w, h * 2 / 3, l / 3), 
            new Vector3(w, h * 2 / 3, l * 2 / 3), new Vector3(0, h * 2 / 3, l * 2 / 3),
            // Front face
            new Vector3(w, h * 2 / 3, l * 2 / 3), new Vector3(0, h * 2 / 3, l * 2 / 3),
            new Vector3(0, h / 3, l * 2 / 3), new Vector3(w, h / 3, l * 2 / 3),
            // Left face
            new Vector3(0, 0, l / 3), new Vector3(0, 0, l * 2 / 3), 
            new Vector3(0, h * 2 / 3, l / 3), new Vector3(0, h * 2 / 3, l * 2 / 3), 
            // Right face
            new Vector3(w, 0, l / 3), new Vector3(w, 0, l * 2 / 3), 
            new Vector3(w, h * 2 / 3, l / 3), new Vector3(w, h * 2 / 3, l * 2 / 3),
            
            // Bottom Step -----------------------------------------------------------
            
            // Top face
            new Vector3(0, h / 3, l * 2 / 3), new Vector3(w, h / 3, l * 2 / 3), 
            new Vector3(w, h / 3, l), new Vector3(0, h / 3, l),
            // Front face
            new Vector3(w, h / 3, l), new Vector3(0, h / 3, l),
            new Vector3(0, 0, l), new Vector3(w, 0, l),
            // Left face
            new Vector3(0, 0, l * 2 / 3), new Vector3(0, 0, l),
            new Vector3(0, h / 3, l * 2 / 3), new Vector3(0, h / 3, l),
            // Right face
            new Vector3(w, 0, l * 2 / 3), new Vector3(w, 0, l),
            new Vector3(w, h / 3, l * 2 / 3), new Vector3(w, h / 3, l),
            
            // Bottom face
            new Vector3(0, 0, 0), new Vector3(w, 0, 0), 
            new Vector3(w, 0, l), new Vector3(0, 0, l),
            // Back face
            new Vector3(0, 0, 0), new Vector3(w, 0, 0), 
            new Vector3(w, h, 0), new Vector3(0, h, 0)
        };
    }
    
    public int[][] GetTriangles() {
        return new[] {
            // Top Step ------------------------------------------------------------
                
            // Top face
            new[] { 3, 2, 1, 3, 1, 0 },
            // Front face
            new[] { 6, 7, 4, 6, 4, 5 },
            // Left face
            new[] { 8, 9, 11, 8, 11, 10 },
            // Right face
            new[] { 13, 12, 14, 13, 14, 15 },
                
            // Mid Step -------------------------------------------------------------
                
            // Top face
            new[] { 19, 18, 17, 19, 17, 16 },
            // Front face
            new[] { 22, 23, 20, 22, 20, 21 },
            // Left face
            new[] { 24, 25, 27, 24, 27, 26 },
            // Right face
            new[] { 29, 28, 30, 29, 30, 31 },
                
            // Bottom Step -----------------------------------------------------------
                
            // Top face
            new[] { 35, 34, 33, 35, 33, 32 },
            // Front face
            new[] { 38, 39, 36, 38, 36, 37 },
            // Left face
            new[] { 40, 41, 43, 40, 43, 42 },
            // Right face
            new[] { 45, 44, 46, 45, 46, 47 },
                
            // Bottom face
            new[] { 48, 49, 50, 48, 50, 51 },
            // Back face
            new[] { 52, 54, 53, 52, 55, 54 }
        };
    }

    public Vector2[] GetUVMapping() {
        return new[] {
            // Top Step ------------------------------------------------------------

            // Top face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Front face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Left face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Right face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Mid Step -------------------------------------------------------------

            // Top face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Front face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Left face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Right face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),

            // Bottom Step -----------------------------------------------------------

            // Top face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Front face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Left face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Right face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),

            // Bottom face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),
            // Back face
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1)
        };
    }
}
