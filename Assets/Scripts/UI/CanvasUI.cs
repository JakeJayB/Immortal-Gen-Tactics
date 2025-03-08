using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CanvasUI : MonoBehaviour
{
    private void Awake()
    {
        UnitMenu.Initialize(this.gameObject);
        UnitSelector.Initialize(this.gameObject);
    }
}
