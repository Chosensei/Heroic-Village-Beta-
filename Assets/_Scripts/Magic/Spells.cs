using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spells
{
    [SerializeField] private string name;
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float castTime;
    [SerializeField] private GameObject spellPrefab;

    public string Name { get => name; }
    public int Damage { get => damage; }
    public float Speed { get => speed; }
    public float CastTime { get => castTime; }
    public GameObject SpellPrefab { get => spellPrefab; }
}


