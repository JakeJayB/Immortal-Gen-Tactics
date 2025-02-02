using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileProperties
{
    public const float TILE_WIDTH = 0.5f;
    public const float TILE_LENGTH = 0.5f;
    public const float TILE_HEIGHT = 0.25f;

    public static int GetFaceCount(TileType tileType)
    {
        return tileType switch
        {
            TileType.Flat => 6,
            TileType.Slanted => 5,
            TileType.Slanted_Corner => 5,
            TileType.Stairs => 14,
            _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
        };
    }
    
    public static Vector3[] GetVertices(TileType tileType)
    {
        return tileType switch
        {
            TileType.Flat => new[]
            {
                // Front face
                new Vector3(0, 0, TILE_LENGTH), new Vector3(TILE_WIDTH, 0, TILE_LENGTH),
                new Vector3(TILE_WIDTH, TILE_HEIGHT, TILE_LENGTH), new Vector3(0, TILE_HEIGHT, TILE_LENGTH),
                // Back face
                new Vector3(0, 0, 0), new Vector3(TILE_WIDTH, 0, 0),
                new Vector3(TILE_WIDTH, TILE_HEIGHT, 0), new Vector3(0, TILE_HEIGHT, 0),
                // Top face
                new Vector3(0, TILE_HEIGHT, 0) , new Vector3(TILE_WIDTH, TILE_HEIGHT, 0),
                new Vector3(TILE_WIDTH, TILE_HEIGHT, TILE_LENGTH), new Vector3(0, TILE_HEIGHT, TILE_LENGTH),
                // Bottom face
                new Vector3(0, 0, 0), new Vector3(TILE_WIDTH, 0, 0),
                new Vector3(TILE_WIDTH, 0, TILE_LENGTH), new Vector3(0, 0, TILE_LENGTH),
                // Left face
                new Vector3(0, 0, 0), new Vector3(0, 0, TILE_LENGTH),
                new Vector3(0, TILE_HEIGHT, TILE_LENGTH), new Vector3(0, TILE_HEIGHT, 0),
                // Right face
                new Vector3(TILE_WIDTH, 0, 0), new Vector3(TILE_WIDTH, 0, TILE_LENGTH),
                new Vector3(TILE_WIDTH, TILE_HEIGHT, TILE_LENGTH), new Vector3(TILE_WIDTH, TILE_HEIGHT, 0)
            },
            TileType.Slanted => new[]
            {
                // Back face
                new Vector3(0, 0, 0), new Vector3(TILE_WIDTH, 0, 0), 
                new Vector3(TILE_WIDTH, TILE_HEIGHT, 0), new Vector3(0, TILE_HEIGHT, 0),
                // Top face
                new Vector3(0, TILE_HEIGHT, 0), new Vector3(TILE_WIDTH, TILE_HEIGHT, 0), 
                new Vector3(TILE_WIDTH, 0, TILE_LENGTH), new Vector3(0, 0, TILE_LENGTH),
                // Bottom face
                new Vector3(0, 0, 0), new Vector3(TILE_WIDTH, 0, 0), 
                new Vector3(TILE_WIDTH, 0, TILE_LENGTH), new Vector3(0, 0, TILE_LENGTH),
                // Left face
                new Vector3(0, 0, 0), new Vector3(0, 0, TILE_LENGTH), 
                new Vector3(0, TILE_HEIGHT, 0),
                // Right face
                new Vector3(TILE_WIDTH, 0, 0), new Vector3(TILE_WIDTH, 0, TILE_LENGTH), 
                new Vector3(TILE_WIDTH, TILE_HEIGHT, 0)
            },
            TileType.Slanted_Corner => new[]
            {
                // Back face
                new Vector3(0, 0, 0), new Vector3(TILE_WIDTH, 0, 0), 
                new Vector3(0, TILE_HEIGHT, 0),
                // Top-Left face
                new Vector3(0, TILE_HEIGHT, 0), new Vector3(TILE_WIDTH, 0, 0), 
                new Vector3(TILE_WIDTH, 0, TILE_LENGTH),
                // Top-Right Face
                new Vector3(0, TILE_HEIGHT, 0), new Vector3(0, 0, TILE_LENGTH), 
                new Vector3(TILE_WIDTH, 0, TILE_LENGTH),
                // Bottom face
                new Vector3(0, 0, 0), new Vector3(TILE_WIDTH, 0, 0), 
                new Vector3(TILE_WIDTH, 0, TILE_LENGTH), new Vector3(0, 0, TILE_LENGTH),
                // Left face
                new Vector3(0, 0, 0), new Vector3(0, 0, TILE_LENGTH), 
                new Vector3(0, TILE_HEIGHT, 0)
            },
            TileType.Stairs => new[]
            {
                // Top Step ------------------------------------------------------------
                
                // Top face
                new Vector3(0, TILE_HEIGHT, 0), new Vector3(TILE_WIDTH, TILE_HEIGHT, 0), 
                new Vector3(TILE_WIDTH, TILE_HEIGHT, TILE_LENGTH / 3), new Vector3(0, TILE_HEIGHT, TILE_LENGTH / 3),
                // Front face
                new Vector3(TILE_WIDTH, TILE_HEIGHT, TILE_LENGTH / 3), new Vector3(0, TILE_HEIGHT, TILE_LENGTH / 3),
                new Vector3(0, TILE_HEIGHT * 2 / 3, TILE_LENGTH / 3), new Vector3(TILE_WIDTH, TILE_HEIGHT * 2 / 3, TILE_LENGTH / 3),
                // Left face
                new Vector3(0, 0, 0), new Vector3(0, 0, TILE_LENGTH / 3), 
                new Vector3(0, TILE_HEIGHT, 0), new Vector3(0, TILE_HEIGHT, TILE_LENGTH / 3),
                // Right face
                new Vector3(TILE_WIDTH, 0, 0), new Vector3(TILE_WIDTH, 0, TILE_LENGTH / 3), 
                new Vector3(TILE_WIDTH, TILE_HEIGHT, 0), new Vector3(TILE_WIDTH, TILE_HEIGHT, TILE_LENGTH / 3),
                
                // Mid Step -------------------------------------------------------------
                
                // Top face
                new Vector3(0, TILE_HEIGHT * 2 / 3, TILE_LENGTH / 3), new Vector3(TILE_WIDTH, TILE_HEIGHT * 2 / 3, TILE_LENGTH / 3), 
                new Vector3(TILE_WIDTH, TILE_HEIGHT * 2 / 3, TILE_LENGTH * 2 / 3), new Vector3(0, TILE_HEIGHT * 2 / 3, TILE_LENGTH * 2 / 3),
                // Front face
                new Vector3(TILE_WIDTH, TILE_HEIGHT * 2 / 3, TILE_LENGTH * 2 / 3), new Vector3(0, TILE_HEIGHT * 2 / 3, TILE_LENGTH * 2 / 3),
                new Vector3(0, TILE_HEIGHT / 3, TILE_LENGTH * 2 / 3), new Vector3(TILE_WIDTH, TILE_HEIGHT / 3, TILE_LENGTH * 2 / 3),
                // Left face
                new Vector3(0, 0, TILE_LENGTH / 3), new Vector3(0, 0, TILE_LENGTH * 2 / 3), 
                new Vector3(0, TILE_HEIGHT * 2 / 3, TILE_LENGTH / 3), new Vector3(0, TILE_HEIGHT * 2 / 3, TILE_LENGTH * 2 / 3), 
                // Right face
                new Vector3(TILE_WIDTH, 0, TILE_LENGTH / 3), new Vector3(TILE_WIDTH, 0, TILE_LENGTH * 2 / 3), 
                new Vector3(TILE_WIDTH, TILE_HEIGHT * 2 / 3, TILE_LENGTH / 3), new Vector3(TILE_WIDTH, TILE_HEIGHT * 2 / 3, TILE_LENGTH * 2 / 3),
                
                // Bottom Step -----------------------------------------------------------
                
                // Top face
                new Vector3(0, TILE_HEIGHT / 3, TILE_LENGTH * 2 / 3), new Vector3(TILE_WIDTH, TILE_HEIGHT / 3, TILE_LENGTH * 2 / 3), 
                new Vector3(TILE_WIDTH, TILE_HEIGHT / 3, TILE_LENGTH), new Vector3(0, TILE_HEIGHT / 3, TILE_LENGTH),
                // Front face
                new Vector3(TILE_WIDTH, TILE_HEIGHT / 3, TILE_LENGTH), new Vector3(0, TILE_HEIGHT / 3, TILE_LENGTH),
                new Vector3(0, 0, TILE_LENGTH), new Vector3(TILE_WIDTH, 0, TILE_LENGTH),
                // Left face
                new Vector3(0, 0, TILE_LENGTH * 2 / 3), new Vector3(0, 0, TILE_LENGTH),
                new Vector3(0, TILE_HEIGHT / 3, TILE_LENGTH * 2 / 3), new Vector3(0, TILE_HEIGHT / 3, TILE_LENGTH),
                // Right face
                new Vector3(TILE_WIDTH, 0, TILE_LENGTH * 2 / 3), new Vector3(TILE_WIDTH, 0, TILE_LENGTH),
                new Vector3(TILE_WIDTH, TILE_HEIGHT / 3, TILE_LENGTH * 2 / 3), new Vector3(TILE_WIDTH, TILE_HEIGHT / 3, TILE_LENGTH),
                
                // Bottom face
                new Vector3(0, 0, 0), new Vector3(TILE_WIDTH, 0, 0), 
                new Vector3(TILE_WIDTH, 0, TILE_LENGTH), new Vector3(0, 0, TILE_LENGTH),
                // Back face
                new Vector3(0, 0, 0), new Vector3(TILE_WIDTH, 0, 0), 
                new Vector3(TILE_WIDTH, TILE_HEIGHT, 0), new Vector3(0, TILE_HEIGHT, 0)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
        };
    }

    public static int[][] GetTriangles(TileType tileType)
    {
        return tileType switch
        {
            TileType.Flat => new[]
            {
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
            },
            TileType.Slanted => new[]
            {
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
            },
            TileType.Slanted_Corner => new[]
            {
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
            },
            TileType.Stairs => new[]
            {
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
            },
            _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
        };
    }

    public static Vector2[] GetUVMapping(TileType tileType)
    {
        return tileType switch
        {
            TileType.Flat => new[]
            {
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
            },
            TileType.Slanted => new[]
            {
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
            },
            TileType.Slanted_Corner => new[]
            {
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
            },
            TileType.Stairs => new[]
            {
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
                new Vector2(1, 1), new Vector2(0, 1),
            },
            _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
        };
    }
}
