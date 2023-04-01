using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
public class BaseStat : MonoBehaviour
{
    public enum BaseStatType { Power, Toughness, AttackSpeed }
    [JsonConverter(typeof(StringEnumConverter))]
    public BaseStatType StatType { get; set; }
    public List<BonusStat>BaseAdditives { get; set; }
    public int BaseValue { get; set; }
    public string StatName { get; set; }
    public string StatDescription { get; set; }
    public int FinalValue { get; set; }
    private BonusStat bonusStat; 
    public BaseStat(int baseValue, string statName, string statDescription)
    {
        BaseAdditives = new List<BonusStat>();
        BaseValue = baseValue;
        StatName = statName;
        StatDescription = statDescription;
    }
    [Newtonsoft.Json.JsonConstructor]
    public BaseStat(BaseStatType statType, int baseValue, string statName)
    {
        BaseAdditives = new List<BonusStat>();
        StatType = statType;
        BaseValue = baseValue;
        StatName = statName;
    }
    public void AddStatBonus(BonusStat bonusStat)
    {
        BaseAdditives.Add(bonusStat);
    }
    public void RemoveStatBonus(BonusStat bonusStat)
    {
        BaseAdditives.Remove(BaseAdditives.Find(x => x.BonusValue == bonusStat.BonusValue));
    }
    // Returns the final calculated value of player stat
    public int GetCalculatedStatValue()
    {
        FinalValue = 0;
        BaseAdditives.ForEach(x => FinalValue += x.BonusValue);
        FinalValue += BaseValue;    // also need to add in the current base value to get total final value
        return FinalValue; 
    }
}
