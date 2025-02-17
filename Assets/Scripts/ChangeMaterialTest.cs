using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeMaterialTest : MonoBehaviour
{
    private bool isTileBlue = false;
    public Material blueMaterial;
    private Dictionary<TileType, HashSet<int>> rangeFinderEdges = new Dictionary<TileType, HashSet<int>>
    {
        { TileType.Flat, new HashSet<int> {2} },
        { TileType.Slanted, new HashSet<int> {1} },
        { TileType.Slanted_Corner, new HashSet<int> {1, 2} },
        { TileType.Stairs, new HashSet<int> {0, 1, 4, 5, 8, 9} }
    };

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isTileBlue)
        {
            foreach(var entry in TilemapCreator.TileLocator)
            {
                Tile tile = entry.Value;
                HashSet<int> edges = rangeFinderEdges[tile.TileInfo.TileType];
                Material[] materials = tile.GameObj.GetComponent<MeshRenderer>().sharedMaterials;

                for (int i = 0; i < materials.Length; i++)
                {
                    if (edges.Contains(i))
                        materials[i] = blueMaterial;
                }
                tile.GameObj.GetComponent<MeshRenderer>().materials = materials;
                
            }
            isTileBlue = true;
        }
        else if(Input.GetKeyDown(KeyCode.Return) && isTileBlue)
        {
            foreach (var entry in TilemapCreator.TileLocator)
            {
                Tile tile = entry.Value;
                int subMeshCount = tile.GameObj.GetComponent<MeshRenderer>().materials.Length;
                tile.GameObj.GetComponent<MeshRenderer>().materials = Terrain.GetTerrain(tile.TileInfo.TileType, tile.TileInfo.TerrainType, subMeshCount);
            }
            isTileBlue = false;
        }
    }
}
