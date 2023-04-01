using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectClosest : MonoBehaviour
{
    const string TARGETTAG = "Cage Wall";

    public GameObject[] AllTargets;
    // for saving the closest target
    public GameObject ClosestTarget { get; set; }
    public GameObject CurrentClosestTarget; 
    void Start()
    {
        // Find all targets
        AllTargets = GameObject.FindGameObjectsWithTag(TARGETTAG);
    }


    void Update()
    {
        CurrentClosestTarget = ClosestTarget; 

        ClosestTarget = FindClosest();
    }
    private GameObject FindClosest()
    {
        GameObject closest = gameObject;
        float leastDist = Mathf.Infinity;

        // Get distance of all targets from enemy and check whose least is the closest one
        foreach (var target in AllTargets)
        {
            // comparing distance 
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < leastDist)
            {
                leastDist = distance;
                closest = target;
            }
        }
        return closest;
    }
}