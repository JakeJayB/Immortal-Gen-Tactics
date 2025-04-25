using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    public static int ProjectDamage(UnitAction action, UnitInfo attacker, UnitInfo target)
    {
        int projected = 0;
        switch (action.DamageType)
        {
            case DamageType.Physical:
                projected = attacker.finalAttack + action.BasePower - target.finalDefense;
                break;
            case DamageType.Magic:
                projected = attacker.finalMagicAttack + action.BasePower - target.finalMagicDefense;
                break;
        }

        return projected > 0 ? projected : 1;
    }
    
    public static int ProjectHealing(UnitAction action, UnitInfo attacker, UnitInfo target)
    {
        int healingAmount = 0;
        
        switch (action.ActionType)
        {
            case ActionType.Attack:
                healingAmount = attacker.finalMagicAttack + action.BasePower;
                break;
            case ActionType.Item:
                healingAmount = action.BasePower;
                break;
            default:
                Debug.LogError("ERROR: ActionType not specified. (DamageCalculator.ProjectHealing)");
                break;
        }
        
        return target.currentHP + healingAmount > target.finalHP ? target.finalHP - target.currentHP : healingAmount;
    }
    
    public static int DealDamage(UnitAction action, UnitInfo attacker, UnitInfo target)
    {
        int damage = 0;
        switch (action.DamageType)
        {
            case DamageType.Physical:
                damage = attacker.finalAttack + action.BasePower - target.finalDefense;
                break;
            case DamageType.Magic:
                damage = attacker.finalMagicAttack + action.BasePower - target.finalMagicDefense;
                break;
        }
        
        damage = ApplyDamageRoll(damage > 0 ? damage : 1);
        target.currentHP -= target.currentHP - damage < 0 ? target.currentHP : damage;


/*        if(attacker.UnitAffiliation == UnitAffiliation.Player)
        {
            CanvasUI.ShowTurnUnitInfoDisplay(attacker);
            CanvasUI.ShowTargetUnitInfoDisplay(target);
        }
        else
        {
            CanvasUI.ShowTurnUnitInfoDisplay(target);
            CanvasUI.ShowTargetUnitInfoDisplay(attacker);
        }*/
        CanvasUI.ShowTurnUnitInfoDisplay(attacker);
        CanvasUI.ShowTargetUnitInfoDisplay(target);

        if (target.currentHP <= 0) { target.Die(); }
        return damage;

    }

    public static void DamageFixedAmount(int amount, UnitInfo target)
    {
        target.currentHP -= amount;
        target.currentHP = target.currentHP < 0 ? 0 : target.currentHP;
    }
    
    public static int HealFixedAmount(int amount, UnitInfo target)
    {
        int initialHP = target.currentHP;
        
        target.currentHP += amount;
        target.currentHP = target.currentHP > target.finalHP ? target.finalHP : target.currentHP;
        CanvasUI.ShowTurnUnitInfoDisplay(target);

        return target.currentHP == target.finalHP ? target.finalHP - initialHP : amount;
    }
    
    public static int HealFixedAmountMP(int amount, UnitInfo target)
    {
        int initialMP = target.currentMP;
        
        target.currentMP += amount;
        target.currentMP = target.currentMP > target.finalMP ? target.finalMP : target.currentMP;

        return target.currentMP == target.finalMP ? target.finalMP - initialMP : amount;
    }

    private static int ApplyDamageRoll(int baseDamage) {
        return (int)(baseDamage + baseDamage * Random.Range(-0.2f, 0.2f));
    }
}
