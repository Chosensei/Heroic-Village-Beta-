using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour, IWeapon
{
    public List<BaseStat> Stats { get; set; }

    public void PerformAttack()
    {
        Debug.Log(this.name + " Attack!");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
