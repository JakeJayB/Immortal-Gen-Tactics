using UnityEngine;

public class TilemapCreator : MonoBehaviour
{

    public GameObject[] tiles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.tiles = GameObject.FindGameObjectsWithTag("Player");

        // Debug.Log("Global Position: " + this.tiles[0].transform.position);
        // Debug.Log("Local Position: " + this.tiles[0].transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
