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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DefineSlot(UnitAction unitAction)
    {
        Name = Action.Name;
        Image image = gameObject.AddComponent<Image>();
        image.sprite = Action.SlotImage();
    }
}
