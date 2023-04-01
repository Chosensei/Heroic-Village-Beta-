using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Player _player; 
    void Start()
    {
        _player = GetComponent<Player>();
    }
    public void SelectEnemy(EnemyTarget target)
    {
        //Debug.Log("Enemy selected");
    }

    public void autoAttack(EnemyTarget target)
    {
        Debug.Log("Started attacking!");
        //bool isInRange = Vector3.Distance(transform.position, _player.SelectEnemy.transform.position);
    }
}
