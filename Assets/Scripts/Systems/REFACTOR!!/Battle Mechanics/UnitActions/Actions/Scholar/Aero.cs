using System.Collections;
using System.Collections.Generic;
using IGT.Core;
using UnityEngine;

namespace IGT.Systems
{
    public class Aero : UnitAction
    {
        public override string Name { get; protected set; } = "Aero";
        public override int MPCost { get; protected set; } = 0;
        public override int APCost { get; protected set; } = 0;
        public override int BasePower { get; protected set; } = 0;
        public override int Range { get; protected set; } = 0;
        public override int Splash { get; protected set; } = 0;
        public override int Priority { get; protected set; } = 0;
        public override DamageType DamageType { get; protected set; } = DamageType.None;
        public override ActionType ActionType { get; protected set; } = ActionType.Attack;
        public override UnitClass ClassType { get; protected set; } = UnitClass.Scholar;
        public override TilePattern AttackTilePattern { get; protected set; } = TilePattern.None;
        public override AIActionScore ActionScore { get; protected set; }
        public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) {
            return null;
        }

        public override string SlotImageAddress { get; protected set; }
        public override float CalculateActionScore(AIUnit unit, Vector2Int selectedCell)
        {
            throw new System.NotImplementedException();
        }

        public override void ActivateAction(Unit unit)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
        {
            throw new System.NotImplementedException();
        }
    }
}
