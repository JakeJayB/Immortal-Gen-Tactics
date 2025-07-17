using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayMaterial
{
    private static readonly Material MoveBlueMat = Resources.Load<Material>("Materials/Move Blue");
    private static readonly Material AttackRedMat = Resources.Load<Material>("Materials/Attack Red");
    private static readonly Material StartBlueMat = Resources.Load<Material>("Materials/Start Blue");
    
    public static Material GetMaterial(OverlayState state) {
        return state switch {
            OverlayState.MOVE => MoveBlueMat,
            OverlayState.ATTACK => AttackRedMat,
            OverlayState.START => StartBlueMat,
            _ => MoveBlueMat
        };
    }
}
