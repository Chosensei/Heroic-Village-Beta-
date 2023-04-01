using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public List<BaseStat> Stats { get; set; }
    void PerformAttack(); 
}
