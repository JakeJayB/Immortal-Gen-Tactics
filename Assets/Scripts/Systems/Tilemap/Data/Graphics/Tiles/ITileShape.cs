using UnityEngine;

public interface ITileShape {
    int GetFaceCount();
    Vector3[] GetVertices();
    int[][] GetTriangles();
    Vector2[] GetUVMapping();
}
