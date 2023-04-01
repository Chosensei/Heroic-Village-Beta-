using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword :  MonoBehaviour, IWeapon
{
    public List<BaseStat> Stats { get; set; }
    public CharacterStats CharacterStats { get; set; }
    private Animator animator;
    private const string BASE_SWORD_ATTACK = "Base_Attack"; 

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update() {}
    public void PerformAttack()
    {
        //Debug.Log(this.name + " Attack!");
        animator.SetTrigger(BASE_SWORD_ATTACK);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<IEnemy>().TakeDamage(CharacterStats.GetStat(BaseStat.BaseStatType.Power).GetCalculatedStatValue());
            Debug.Log("Hit enemy: -" + Stats[0].GetCalculatedStatValue() + " dmg");
        }
        //Debug.Log("Hit: " + other.name);
    }
}
