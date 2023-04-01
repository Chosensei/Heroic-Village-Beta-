using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class can be a way to add / remove bonus stats from player and enemies 
/// Players can reference to this component and adjust its own stats accordingly
/// Enemies will have its own type component and have a reference to characterStat component 
/// & set up its own unique stats
/// </summary>
public class CharacterStats 
{
    public List<BaseStat> Stats = new List<BaseStat>();

    public CharacterStats(int power, int toughness, int attackSpeed)
    {
        Stats = new List<BaseStat>() {
            new BaseStat(BaseStat.BaseStatType.Power, power, "Power"),
            new BaseStat(BaseStat.BaseStatType.Toughness, toughness, "Toughness"),
            new BaseStat(BaseStat.BaseStatType.AttackSpeed, attackSpeed, "Atk Spd")
        };
    }
    // Method returns the basestat to whatever calls it
    public BaseStat GetStat(BaseStat.BaseStatType stat)
    {
        return this.Stats.Find(x => x.StatType == stat);
    }
    // Method will be called when equipping a weapon
    public void AddStatBonus(List<BaseStat> statBonuses)
    {
        foreach(BaseStat bonusStat in statBonuses)
        {
            // find the proper stat we want to add that matches that statbonus attribute
            GetStat(bonusStat.StatType).AddStatBonus(new BonusStat(bonusStat.BaseValue));
        }
    }

    // Method will be called when unequipping a weapon
    public void RemoveStatBonus(List<BaseStat> statBonuses)
    {
        foreach (BaseStat bonusStat in statBonuses)
        {
            // find the proper stat we want to add that matches that statbonus attribute
            GetStat(bonusStat.StatType).RemoveStatBonus(new BonusStat(bonusStat.BaseValue));
        }
    }
}
