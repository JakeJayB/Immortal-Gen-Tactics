using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AITurnHandler : MonoBehaviour {
    public void StartTurn(AIUnit unitAI) {
        CanvasUI.ShowTurnUnitInfoDisplay(unitAI.UnitInfo);
        StartCoroutine(DecideAction(unitAI)); 
    }
    
    public IEnumerator React(AIUnit unitAI) {
        CanvasUI.ShowTurnUnitInfoDisplay(unitAI.UnitInfo);
        yield return StartCoroutine(DecideAction(unitAI)); 
    }
    
    private IEnumerator DecideAction(AIUnit unitAI) {
        var actionDetermined = false;
        var isReacting = ChainSystem.ReactionInProgress;
        
        unitAI.AIConditions = unitAI.AIConditions.OrderBy(a => a.Priority).ToList();
        
        // Decide Target
        unitAI.targetedUnit = unitAI.targetedUnit == null || !unitAI.targetedUnit.GameObj
            ? new AIUnitTargeting().EvaluateScore(unitAI).TargetUnit 
            : unitAI.targetedUnit;

        foreach (var behavior in unitAI.AIConditions) {
            if (!behavior.Condition()) continue;

            actionDetermined = true;
            Debug.Log("AIUnit performing " + behavior.Action.Name + "!");
            ChainSystem.HoldPotentialChain(behavior.Action, unitAI);
            yield return ChainSystem.AddAction(new Vector2Int(unitAI.UnitInfo.CellLocation.x, unitAI.UnitInfo.CellLocation.z));
            break;
        }

        if (!actionDetermined) {
            Debug.Log($"!!!!!!!!!!!!{unitAI.targetedUnit.UnitInfo.Vector2CellLocation()} is the target!!!!!!!!!!!!!!");
            var nearbyUnit = unitAI.targetedUnit.UnitInfo.Vector2CellLocation();
            
            // Softmax Implementation
            List<UnitAction> turnActions = isReacting
                ? unitAI.ActionSet.GetAIReactions()
                : unitAI.ActionSet.GetAITurnActions();
            List<UnitAction> potentialActions = new();
            List<float> actionScores = new List<float>();
            
            foreach (var action in turnActions) {
                if (unitAI.UnitInfo.currentAP < action.APCost || unitAI.UnitInfo.currentMP < action.MPCost) { continue; }

                /*
                if ((targetedUnit.unitInfo.UnitAffiliation == UnitAffiliation.Enemy &&
                     action.DamageType is DamageType.Physical or DamageType.Magic) ||
                    (targetedUnit.unitInfo.UnitAffiliation == UnitAffiliation.Player &&
                     action.DamageType == DamageType.Healing))
                    continue;
                */
                
                potentialActions.Add(action);
                actionScores.Add(action.CalculateActionScore(unitAI, nearbyUnit) / 2); // TODO: Fix Score Balancing to Prevent Softmax Overflow
            }

            UnitAction chosenAction = SoftmaxAILogic.DetermineAction(potentialActions, actionScores);
        
            Debug.Log(unitAI.GameObj.name + " choose to " + chosenAction.Name + " this turn.");
            ChainSystem.HoldPotentialChain(chosenAction, unitAI);
            yield return ChainSystem.AddAction(chosenAction.ActionScore.Vector2PotentialLocation());
            
            // If AI Unit selects an action that impacts the target unit, de-prioritize them
            if (chosenAction.ActionType != ActionType.Move) { unitAI.targetedUnit = null; }
        }

        if (isReacting) {
            unitAI.targetedUnit = null;
            yield return null;
        }
        else if (ChainSystem.Peek().GetType() == typeof(Wait)) {
            yield return new WaitForSeconds(2f);
            yield return ChainSystem.ExecuteChain();
            unitAI.targetedUnit = null;
        }
        else {
            yield return ChainSystem.ExecuteChain();
            if (unitAI.targetedUnit != null && unitAI.targetedUnit.UnitInfo.IsDead()) { unitAI.targetedUnit = null; }
            StartCoroutine(DecideAction(unitAI));
        }
    }
}
