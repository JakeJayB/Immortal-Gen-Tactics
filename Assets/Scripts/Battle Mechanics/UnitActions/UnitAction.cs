using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAction
{
    public abstract string Name { get; protected set; }
    public abstract ActionType ActionType { get; protected set; }
    public abstract string SlotImageAddress { get; protected set; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract Sprite SlotImage();
}
