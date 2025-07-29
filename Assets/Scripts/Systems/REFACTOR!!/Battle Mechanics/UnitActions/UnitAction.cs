using System.Collections;
using System.Collections.Generic;
using IGT.Core;
using UnityEngine;

public abstract class UnitAction {
    public abstract string Name { get; protected set; }
    public abstract int MPCost { get; protected set; }
    public abstract int APCost { get; protected set; }
    public abstract int BasePower { get; protected set; }
    public abstract int Range { get; protected set; }
    public abstract int Splash { get; protected set; }
    public abstract int Priority { get; protected set; }
    public abstract DamageType DamageType { get; protected set; }
    public abstract ActionType ActionType { get; protected set; }
    public abstract UnitClass ClassType { get; protected set; }
    public abstract TilePattern AttackTilePattern { get; protected set; }
    public abstract AIActionScore ActionScore { get; protected set; }
    public abstract List<Tile> Area(Unit unit, Vector3Int? hypoCell);
    public abstract string SlotImageAddress { get; protected set; }

    public abstract float CalculateActionScore(AIUnit unit, Vector2Int selectedCell);
    public abstract void ActivateAction(Unit unit);
    public abstract IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell);

    protected void PayAPCost(Unit unit) { unit.UnitInfo.currentAP -= APCost; }
    protected void PayMPCost(Unit unit) { unit.UnitInfo.currentMP -= MPCost; }
    public Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
}
