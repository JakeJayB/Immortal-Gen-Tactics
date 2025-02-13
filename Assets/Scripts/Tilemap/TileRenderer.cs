using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class TileRenderer : MonoBehaviour
{

    public void Render(Vector3Int cellLocation, TileType tileType, TerrainType terrainType, TileDirection direction)
    {

        Mesh mesh = new Mesh
        {
            vertices = TileProperties.GetVertices(tileType),
            uv = TileProperties.GetUVMapping(tileType)
        };

        // Center the pivots of each tile
        mesh.vertices = CenterPivots(mesh.vertices);

        // Triangles for each face
        int[][] triangles = TileProperties.GetTriangles(tileType);

        // Assign triangles to sub-meshes
        mesh.subMeshCount = TileProperties.GetFaceCount(tileType);
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            mesh.SetTriangles(triangles[i], i);
        }

        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.materials = Terrain.GetTerrain(tileType, terrainType, mesh.subMeshCount);

        PositionTile(cellLocation);
        RotateTile(direction);
    }

    private Vector3[] CenterPivots(Vector3[] vertices)
    {
        Vector3 pivotOffset = new Vector3(TileProperties.TILE_WIDTH / 2, TileProperties.TILE_HEIGHT / 2, TileProperties.TILE_LENGTH / 2);

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] -= pivotOffset;
        }

        return vertices;
    }

    private void PositionTile(Vector3Int cellLocation)
    {
        gameObject.transform.position = new Vector3(
            cellLocation.x * TileProperties.TILE_WIDTH,
            cellLocation.y * TileProperties.TILE_HEIGHT,
            cellLocation.z * TileProperties.TILE_LENGTH);
    }

    private void RotateTile(TileDirection direction)
    {
        var rotation = direction switch
        {
            TileDirection.Forward => 0f,
            TileDirection.Backward => 180f,
            TileDirection.Left => 90f,
            TileDirection.Right => 270f,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
        };

        gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
    }


}
