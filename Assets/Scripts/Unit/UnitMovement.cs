using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private const float TRAVEL_SPEED = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            MoveUnit(gameObject.GetComponent<Unit>(), new Vector2Int(0, 1));
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            MoveUnit(gameObject.GetComponent<Unit>(), new Vector2Int(0, -1));
        }
    }

    public static void MoveUnit(Unit unit, Vector2Int direction)
    {
        var destination = Tilemap.TileLocator[new Vector2Int(unit.CellLocation.x, unit.CellLocation.z) + direction];
        if (destination != null)
        {
           unit.gameObject.transform.position = new Vector3(
               (destination.TileInfo.CellLocation.x) * TileProperties.TILE_WIDTH,
               (destination.TileInfo.CellLocation.y) * TileProperties.TILE_HEIGHT + TileProperties.TILE_HEIGHT, 
               (destination.TileInfo.CellLocation.z) * TileProperties.TILE_LENGTH);

           unit.CellLocation = destination.TileInfo.CellLocation;
        }
    }
}
