using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Scriptables/Enemy Data", order = 0)]
public class EnemyData : ScriptableObject
{
    [Header("Enemy stats")]
    public int maxHealth;

    public float currentHealth;

    public int minDamage;

    public int maxDamage;

    public float atkRange;

    public float atkSpeed;

    public float timeBetweenAtks;

    public int killReward;

    public float moveSpeed;
}
