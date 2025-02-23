using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeMaterialTest : MonoBehaviour
{
    private bool isTileBlue = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isTileBlue)
        {
            foreach (var entry in TilemapCreator.TileLocator)
            {
                Tile tile = entry.Value;
                tile.OverlayObj.ActivateOverlayTile(OverlayMaterial.MOVE);
            }
            isTileBlue = true;
        }
        else if (Input.GetKeyDown(KeyCode.Return) && isTileBlue)
        {
            foreach (var entry in TilemapCreator.TileLocator)
            {
                Tile tile = entry.Value;
                tile.OverlayObj.DeactivateOverlyTile();
            }
            isTileBlue = false;
        }
    }
}
