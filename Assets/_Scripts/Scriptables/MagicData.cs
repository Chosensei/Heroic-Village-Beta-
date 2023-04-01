using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Spell", menuName = "Scriptables/New_Magic", order = 1)]
public class MagicData : ScriptableObject
{
    [Header("Magic Attributes")]
    [Tooltip("What is the name of this spell?")][SerializeField] private string name;
    [Tooltip("How much damage this spell does")][SerializeField] private int damage;
    [Tooltip("How fast this spell fly")][SerializeField] private float speed;
    [Tooltip("How long this spell takes to cast")][SerializeField] private float castTime;
    //[SerializeField] private Sprite castTimerImg;
    [Tooltip("Requires a spell prefab asset")][SerializeField] private GameObject spellPrefab;

    public string Name { get => name; }
    public int Damage { get => damage; }
    public float Speed { get => speed; }
    public float CastTime { get => castTime; }
    //public Sprite CastTimerImg { get => castTimerImg; }
    public GameObject SpellPrefab { get => spellPrefab; }

}
