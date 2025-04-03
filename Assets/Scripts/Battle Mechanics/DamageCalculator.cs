using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator
{
    public static int ProjectDamage(UnitAction action, UnitInfo attacker, UnitInfo target)
    {
        switch (action.DamageType)
        {
            case DamageType.Physical:
                return attacker.finalAttack + action.BasePower - target.finalDefense;
            case DamageType.Magic:
                return attacker.finalMagicAttack + action.BasePower - target.finalMagicDefense;
        }

        return 0;
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
    
    public static void DealDamage(DamageType damageType, UnitInfo attacker, UnitInfo target)
    {
        int damage = 0;
        switch (damageType)
        {
            case DamageType.Physical:
                damage = ApplyDamageRoll(attacker.finalAttack - target.finalDefense);
                break;
            case DamageType.Magic:
                damage = ApplyDamageRoll(attacker.finalMagicAttack - target.finalMagicDefense);
                break;
        }
        
        target.currentHP -= target.currentHP - damage < 0 ? target.currentHP : damage;
    }

    public static void DamageFixedAmount(int amount, UnitInfo target)
    {
        target.currentHP -= amount;
        target.currentHP = target.currentHP < 0 ? 0 : target.currentHP;
    }
    
    public static void HealFixedAmount(int amount, UnitInfo target)
    {
        target.currentHP += amount;
        target.currentHP = target.currentHP > target.finalHP ? target.finalHP : target.currentHP;
    }

    private static int ApplyDamageRoll(int baseDamage) {
        return (int)(baseDamage + baseDamage * Random.Range(-1.2f, 1.2f));
    }
}
