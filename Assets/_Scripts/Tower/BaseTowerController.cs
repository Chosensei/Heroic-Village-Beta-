using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class BaseTowerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;
    public float launchForce;
    protected Coroutine shootRoutine;
    protected bool isShooting;
    protected Collider targetCollider;
    protected TowerLevelSwitch tls;

    /// <summary>
    /// The initialization will be different for each type of projectile,
    /// which can be overwritten in their own class scripts
    /// </summary>
    /// <param name="target"></param>
    public virtual IEnumerator Shoot(Transform target)
    {
        throw new NotImplementedException(); 
    }
    protected virtual void Awake()
    {
        tls = new TowerLevelSwitch();
        tls.baseFiringRange = GetComponent<SphereCollider>().radius;
    }
    protected virtual void Update()
    {
        if (targetCollider == null && isShooting)
        {
            StopShooting();
        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && shootRoutine == null)
        {
            targetCollider = other;
            shootRoutine = StartCoroutine(Shoot(other.transform));
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other == targetCollider)
        {
            StopShooting();
        }
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && shootRoutine == null)
        {
            targetCollider = other;
            shootRoutine = StartCoroutine(Shoot(other.transform));
        }
    }

    protected virtual void StopShooting()
    {
        StopCoroutine(shootRoutine);
        isShooting = false;
        targetCollider = null;
        shootRoutine = null;
    }
}
