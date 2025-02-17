using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialTest : MonoBehaviour
{
    private bool isTileBlue = false;
    public Material blueMaterial;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isTileBlue)
        {
            Debug.Log("test");
            isTileBlue = true;
            foreach(var entry in TilemapCreator.TileLocator)
            {
                Tile tile = entry.Value;
                Material[] materials = tile.GameObj.GetComponent<MeshRenderer>().materials;
                for (int i = 0; i < materials.Length; i++)
                    materials[i] = blueMaterial;
                tile.GameObj.GetComponent<MeshRenderer>().materials = materials;
            }
        }
        else if(Input.GetKeyDown(KeyCode.Return) && isTileBlue)
        {
            Debug.Log("test2");
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
