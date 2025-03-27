using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator
{
    public static void DealPhysicalDamage(UnitInfo attacker, UnitInfo target) 
    {
        int damage = ApplyDamageRoll(attacker.finalAttack - target.finalDefense);
        target.currentHP -= target.currentHP - damage < 0 ? target.currentHP : damage;
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
