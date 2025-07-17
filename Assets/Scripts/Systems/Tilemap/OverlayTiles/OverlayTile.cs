using System.Collections.Generic;
using UnityEngine;

public class OverlayTile {
    public GameObject OverlayObj;
    private TileType TileType;
    private OverlayState State;
    public bool IsSelectable;

    public OverlayTile(GameObject OriginalTile, GameObject OverlayTilePrefab) {
        OverlayObj = new GameObject("Overlay Tile for " + OriginalTile.name);
        OverlayObj.SetActive(false);
        OverlayObj.transform.position = new Vector3(OriginalTile.transform.position.x, 
            OriginalTile.transform.position.y + OverlayTilePrefab.transform.position.y, 
            OriginalTile.transform.transform.position.z);
        OverlayObj.transform.rotation = OriginalTile.transform.rotation;
        OverlayObj.transform.localScale = OverlayTilePrefab.transform.localScale;

        MeshFilter OverlayMF = OverlayObj.AddComponent<MeshFilter>();
        OverlayMF.mesh = OverlayTilePrefab.GetComponent<MeshFilter>().sharedMesh;

        MeshRenderer OverlayMR = OverlayObj.AddComponent<MeshRenderer>();
        OverlayMR.materials = OverlayTilePrefab.GetComponent<MeshRenderer>().sharedMaterials;
        OverlayMR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        TileType = OriginalTile.GetComponent<TileInfo>().TileType;
        State = OverlayState.MOVE;
    }

    public void ActivateOverlayTile(OverlayState state) {
        if (State != state) {
            State = state;
            Material newMat = OverlayMaterial.GetMaterial(state);
            HashSet<int> tileSides = OverlayTileHashset.GetHashset(TileType);
            Material[] overlayMats = OverlayObj.GetComponent<MeshRenderer>().sharedMaterials;

            for (int i = 0; i < overlayMats.Length; i++) {
                if (tileSides.Contains(i)) overlayMats[i] = newMat;
            }
            OverlayObj.GetComponent<MeshRenderer>().materials = overlayMats;
        }

        OverlayObj.SetActive(true);
        IsSelectable = true;
    }

    public void DeactivateOverlayTile() {
        OverlayObj.SetActive(false);
        IsSelectable = false;
    }
}
