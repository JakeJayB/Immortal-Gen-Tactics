using UnityEngine;

public class TileReference : MonoBehaviour {
    public Tile Tile { get; private set; }
    public void Reference(Tile tile) { Tile = tile; }
}
