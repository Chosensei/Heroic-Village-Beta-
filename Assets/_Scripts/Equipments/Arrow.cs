using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector3 Direction { get; set; }
    public float Range { get; set; }
    public float Damage { get; set; }
    public float Speed { get; set; }
    Vector3 spawnPosition; 
    private void Start()
    {
        // Setting temporary basic stats here
        Speed = 100f;
        Damage = 2f;
        Range = 100f;
        spawnPosition = transform.position;
        GetComponent<Rigidbody>().AddForce(Direction * Speed);
    }
    private void Update()
    {
        if (Vector3.Distance(spawnPosition, transform.position) >= Range)
        {
            ExtinguishArrow();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<IEnemy>().TakeDamage(Damage);
        }
        //Debug.Log("Hit: " + other.name);
        ExtinguishArrow();
    }
    private void ExtinguishArrow()
    {
        Destroy(this.gameObject);
    }
}
