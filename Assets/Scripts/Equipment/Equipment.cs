using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    // Weapon Info
    private int maxDurability;
    private int durability;
    private int range;
}
public class ArmorHead : Equipment { }
public class ArmorBody : Equipment { }
public class ArmorHands : Equipment { }
public class ArmorFeet : Equipment { }
public class Accessory : Equipment { }

public class Equipment : MonoBehaviour
{
    // Equipment Info
    protected string equipName;
    
    // Equipment Stat Values
    protected int equipHP;
    protected int equipMP;
    protected int equipAP;
    protected int equipAttack;
    protected int equipMagicAttack;
    protected int equipDefense;
    protected int equipMagicDefense;
    protected int equipMove;
    protected int equipSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}