using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSlot : MonoBehaviour
{
    public string Name { get; private set; }
    public UnitAction Action { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        //Name = Action.Name;
        //Image image = gameObject.AddComponent<Image>();
        //image.sprite = Action.SlotImage; // Set the sprite as a UI element
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
