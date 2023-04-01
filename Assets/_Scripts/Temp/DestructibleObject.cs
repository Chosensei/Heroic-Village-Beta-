using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private GameObject fractured;
    [SerializeField] private float breakForce = 2f; 
    void Update()
    {
        if (Input.GetKeyDown("f"))
            BreakHouse(); 
    }
    private void BreakHouse()
    {
        Instantiate(fractured, transform.position, transform.rotation);
        foreach(Rigidbody rb in fractured.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 force = (rb.transform.position - transform.position).normalized * breakForce; 
            rb.AddForce(force); 
        }
        Destroy(this.gameObject); 
    }
}
