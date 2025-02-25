using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum OverlayMaterial
{
    MOVE,
    ATTACK
}

public class OverlayTile
{

    private static Material MoveBlueMat = Resources.Load<Material>("Materials/Move Blue");
    private static Material AttackRedMat = Resources.Load<Material>("Materials/Attack Red");
    private static Dictionary<TileType, HashSet<int>> TileSides = new Dictionary<TileType, HashSet<int>>
    {
        { TileType.Flat, new HashSet<int> {2} },
        { TileType.Slanted, new HashSet<int> {1} },
        { TileType.Slanted_Corner, new HashSet<int> {1, 2} },
        { TileType.Stairs, new HashSet<int> {0, 4, 8} }
    };

    private TileType TileType;
    private OverlayMaterial CurrentMaterial;

    public GameObject OverlayObj;

    public OverlayTile(GameObject OriginalTile, GameObject OverlayTilePrefab)
    {
        OverlayObj = new GameObject("Overaly Tile for " + OriginalTile.name);

        OverlayObj.SetActive(false);
        OverlayObj.transform.position = new Vector3(OriginalTile.transform.position.x, OriginalTile.transform.position.y + OverlayTilePrefab.transform.position.y, OriginalTile.transform.transform.position.z);
        OverlayObj.transform.rotation = OriginalTile.transform.rotation;
        OverlayObj.transform.localScale = OverlayTilePrefab.transform.localScale;

        // Overlay Tile is OverlayMaterial.Move (blue) by default
        MeshFilter OverlayMF = OverlayObj.AddComponent<MeshFilter>();
        OverlayMF.mesh = OverlayTilePrefab.GetComponent<MeshFilter>().sharedMesh;

        MeshRenderer OverlayMR = OverlayObj.AddComponent<MeshRenderer>();
        OverlayMR.materials = OverlayTilePrefab.GetComponent<MeshRenderer>().sharedMaterials;
        OverlayMR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        TileType = OriginalTile.GetComponent<TileInfo>().TileType;
        CurrentMaterial = OverlayMaterial.MOVE;
    }

    public void ActivateOverlayTile(OverlayMaterial mat)
    {

        if (CurrentMaterial != mat)
        {
            Debug.Log("OverlayTile: Changing Material");
            CurrentMaterial = mat;
            Material newMat = mat == OverlayMaterial.ATTACK ? AttackRedMat : MoveBlueMat;
            
            HashSet<int> tileSides = TileSides[TileType];
            Material[] overlayMats = OverlayObj.GetComponent<MeshRenderer>().sharedMaterials;

            for (int i = 0; i < overlayMats.Length; i++)
            {
                if (tileSides.Contains(i))
                    overlayMats[i] = newMat;
            }
            OverlayObj.GetComponent<MeshRenderer>().materials = overlayMats;

        }

        OverlayObj.SetActive(true);
    }

    public void DeactivateOverlyTile()
    {
        OverlayObj.SetActive(false);
    }
}
