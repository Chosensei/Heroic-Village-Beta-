using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosMeteor : MonoBehaviour
{
    public Vector3 spawnOffset = new Vector3(0,1,0);
    public float flyDuration, travelDistance, flySpeed, groundSpeed, meteorSize, spinSpeed, pulseTime, pulseRadius, impactFXDestroyDelay;
    public LayerMask groundLayer;
    public bool visualizeRadius;

    GameObject impactFX;
    Vector3 moveDirection, landLocation;
    float pulseTimer;
    bool flying = true;

    public List<Transform> brokenMeteors = new List<Transform>();

    void Start()
    {
        impactFX = transform.Find("ImpactFX").gameObject;
        moveDirection = transform.position - (transform.position + spawnOffset);
        //Position
        transform.position = transform.position + spawnOffset;
        //Rotation
        Vector3 relativePos = moveDirection - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;

        foreach (Transform child in transform) { brokenMeteors.Add(child); }

        brokenMeteors.Remove(impactFX.transform);
    }

    void Update()
    {
        RaycastHit hit;
        if (flying)
        {
            if(flyDuration > 0) { flyDuration -= Time.deltaTime; } else { Destroy(gameObject); }
            transform.position = transform.position + moveDirection * flySpeed * Time.deltaTime;
            if(Physics.Raycast(transform.position, Vector3.down, out hit, meteorSize, groundLayer))
            {
                flying = false;
                moveDirection.y = 0;
                landLocation = hit.point;
                impactFX.SetActive(true);
                impactFX.transform.SetParent(null);
                Destroy(impactFX, impactFXDestroyDelay);
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, meteorSize, groundLayer))
            {
                Vector3 nextPosition = new Vector3(transform.position.x, hit.point.y + meteorSize, transform.position.z);
                transform.position = nextPosition + moveDirection * groundSpeed * Time.deltaTime;
            }

            float dist = Vector3.Distance(landLocation, transform.position);
            if (dist > travelDistance) 
            {
                foreach (Transform child in brokenMeteors)
                {
                    child.gameObject.SetActive(true);
                    child.SetParent(null);
                    child.GetComponent<Rigidbody>().AddForce(moveDirection * groundSpeed, ForceMode.Impulse);
                    child.GetComponent<Rigidbody>().AddTorque(new Vector3(-100,0,0), ForceMode.Impulse);
                    Destroy(child.gameObject, 3); // clear debris after 3s
                }

                Destroy(gameObject); 
            }

            float spin = spinSpeed * Time.deltaTime;
            transform.Rotate(spin, 0,0);
            DamagePulses();
        }
    }

    void DamagePulses()
    {
        if(pulseTimer > 0) { pulseTimer -= Time.deltaTime; } else
        {
            Collider[] gameObjectsInRange = Physics.OverlapSphere(transform.position, pulseRadius);

            foreach(Collider col in gameObjectsInRange)
            {
                //Check if it has your ENEMY script
                Rigidbody enemy = col.GetComponent<Rigidbody>();

                if(enemy != null)
                {
                    //DAMAGE YOUR ENEMY
                    //enemy.Damage(15);
                }
            }

            pulseTimer = pulseTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (visualizeRadius)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, pulseRadius);
        }
    }
}
