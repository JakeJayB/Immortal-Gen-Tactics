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
                projected = attacker.FinalAttack + action.BasePower - target.FinalDefense;
                break;
            case DamageType.Magic:
                projected = attacker.FinalMagicAttack + action.BasePower - target.FinalMagicDefense;
                break;
            case DamageType.Healing:    // Will need revision when introducing damage gitdealt to undead
                return (attacker.FinalMagicAttack + action.BasePower) * -1;
                break;
            case DamageType.Revival:    // Will need revision when introducing damage dealt to undead
                if (target.IsDead()) { return (attacker.FinalMagicAttack + action.BasePower + target.FinalHP) * -1; }
                break;
        }

        return projected > 0 ? projected : 1;
    }
    
    public static int ProjectHealing(UnitAction action, UnitInfo attacker, UnitInfo target)
    {
        // Dead units cannot be healed, they must be revived first
        if (target.IsDead() && action.DamageType == DamageType.Healing) { return 0; }
        int healingAmount = 0;
        
        switch (action.ActionType)
        {
            case ActionType.Attack:
                healingAmount = attacker.FinalMagicAttack + action.BasePower;
                if (action.DamageType != DamageType.Healing) { healingAmount *= -1; }
                break;
            case ActionType.Item:
                healingAmount = action.BasePower;
                break;
            default:
                //Debug.LogError("ERROR: ActionType not specified. (DamageCalculator.ProjectHealing)");
                break;
        }
        
        return target.currentHP + healingAmount > target.FinalHP ? target.FinalHP - target.currentHP : healingAmount;
    }
    
    public static int DealDamage(UnitAction action, UnitInfo attacker, UnitInfo target)
    {
        if (target.IsDead()) { return 0; }
        
        int damage = 0;
        switch (action.DamageType)
        {
            case DamageType.Physical:
                damage = attacker.FinalAttack + action.BasePower - target.FinalDefense;
                break;
            case DamageType.Magic:
                damage = attacker.FinalMagicAttack + action.BasePower - target.FinalMagicDefense;
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
    
    public static int HealDamage(UnitAction action, UnitInfo attacker, UnitInfo target)
    {
        if (target.IsDead()) { return 0; }
        if (target.currentHP == target.FinalHP) { return 0; }
        
        int damage = attacker.FinalMagicAttack + action.BasePower;
        damage = ApplyDamageRoll(damage > 0 ? damage : 1);
        target.currentHP += target.currentHP + damage > target.FinalHP ? target.FinalHP - target.currentHP : damage;
        
        CanvasUI.ShowTurnUnitInfoDisplay(attacker);
        CanvasUI.ShowTargetUnitInfoDisplay(target);
        
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
        target.currentHP = target.currentHP > target.FinalHP ? target.FinalHP : target.currentHP;
        CanvasUI.ShowTurnUnitInfoDisplay(target);

        return target.currentHP == target.FinalHP ? target.FinalHP - initialHP : amount;
    }
    
    public static int HealFixedAmountMP(int amount, UnitInfo target)
    {
        int initialMP = target.currentMP;
        
        target.currentMP += amount;
        target.currentMP = target.currentMP > target.FinalMP ? target.FinalMP : target.currentMP;

        return target.currentMP == target.FinalMP ? target.FinalMP - initialMP : amount;
    }

    private static int ApplyDamageRoll(int baseDamage) {
        return (int)(baseDamage + baseDamage * Random.Range(-0.2f, 0.2f));
    }
}
