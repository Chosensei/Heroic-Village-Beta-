using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Tooltip("The players current level")]
    public int level;

    [Space]
    [Tooltip("The players / enemy current health")]
    public int currentHP;
    [Tooltip("The players / enemy maximum health")]
    public int maxHP;   

    [Space]
    [Tooltip("The players current mana")]
    public int currentMana;
    [Tooltip("The players maximum mana")]
    public int maxMana;

    [Space]
    [Tooltip("The players current exp")]
    public int currentExp;
    [Tooltip("The players max exp for this level")]
    public int maxExp;
}
