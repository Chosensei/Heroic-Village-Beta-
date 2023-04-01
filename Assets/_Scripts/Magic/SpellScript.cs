using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SphereCollider))]
public class SpellScript : MonoBehaviour
{
    [SerializeField] MagicData magicData;
    private Rigidbody _rb;
    //private Animator _anim; 
    private SphereCollider _sphereCol;

    // The Spells target
    public Transform SpellTarget { get; set; }

    void Awake()
    {
        //_anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.freezeRotation = true;
        _sphereCol = GetComponent<SphereCollider>();
        _sphereCol.isTrigger = true;
    }
    void Start()
    {
        // Auto destroy spell gameobject after 4s
        Destroy(this.gameObject, 4f); 
    }
    void FixedUpdate()
    {
        if (SpellTarget != null)
            FireProjectile();
    }

    private void FireProjectile()
    {
        Vector3 dir = SpellTarget.position - transform.position;
        _rb.velocity = dir * magicData.Speed;
        Debug.Log("Spell velocity: " + magicData.Speed);
        // rotate gameobject to hit target
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && other.transform == SpellTarget)
        {
            _rb.velocity = Vector2.zero;
            SpellTarget = null;

            // Enemy damage function
            EnemyTarget enemy = other.gameObject.GetComponent<EnemyTarget>();
            enemy.TakeDamage(magicData.Damage);
            Debug.Log("Spell damage: " + magicData.Damage);
            Destroy(this.gameObject);
        }
    }
}

