using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusStat : MonoBehaviour
{
    public int BonusValue { get; set; }
    public BonusStat(int bonusValue)
    {
        BonusValue = bonusValue;
        //Debug.Log("New stat bonus initiated"); 
    }
}
