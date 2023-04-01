using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    [SerializeField] private Transform dataHolder; 
    [SerializeField] private Transform zombie;
    [SerializeField] private Transform myZombie;

    private float moveSpeed = 2f;
    private Transform target;
    //private LayerMask hiddenTriggers = 1 << 8; 

    void Start()
    {
        dataHolder = GameObject.Find("PathTriggers").transform;
        myZombie = Instantiate(zombie, transform.position, transform.rotation); 
        
        InvokeRepeating("CheckWalls", 0.5f, 0.5f); 
    }
    void CheckWalls()   // check for nearby walls (or change direction) 
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
