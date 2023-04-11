using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class TowerProjectile : MonoBehaviour
{
    private Transform target;
    private Vector3 initialPosition, targetPosition;
    private float speed;
    private float damage; 
    private float startTime;
    private bool hasInitialized = false;
    private Rigidbody rb;
    private Vector3 forceDirection;
    private TowerLevelSwitch tls; 
    public ProjectileType projectileType;
    public float launchForce;
    public float projectileSpeed = 10f;
    [Header("Cannon Tower Setting")]
    public float explosionRadius = 25;  // the radius of the explosion that deals damage to enemies
    public float cannonForce = 1000f; // the force with which the cannonball hits enemies

    private void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>(); 
        // Auto destroy spell gameobject after 10s
        Destroy(this.gameObject, 20f);
    }
    private void Update()
    {
        if (!hasInitialized) return;
        if (target != null)
        {
            if (projectileType == ProjectileType.Arrow)
            {
                if (target == null) Destroy(gameObject);
                FireArrow();
            }
            else if (projectileType == ProjectileType.Cannonball)
            {
                if (targetPosition == null) Destroy(gameObject);
                FireCannon();

                if (transform.position == targetPosition)
                {
                    DealAOEDamage(tls.baseAoeRadius);
                }
            }
            else if (projectileType == ProjectileType.Iceball)
            {
                if (targetPosition == null) Destroy(gameObject);
                FireArrow();
            }
        }
             
    }
    public void Initialize(Transform target, float speed, float damage)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage; 
        startTime = Time.time;
        hasInitialized = true;
    }
    public void Initialize(Vector3 target, float speed, float damage)
    {
        targetPosition = target;
        this.speed = speed;
        this.damage = damage;
        startTime = Time.time;
        hasInitialized = true;
    }
    private void FireArrow()
    {
        // Calculate direction towards target
        Vector3 direction = (target.transform.position - transform.position).normalized;
        // Move arrow towards target using rigidbody
        rb.velocity = direction * projectileSpeed;
        transform.rotation = Quaternion.LookRotation(direction);
        //transform.position = Vector3.MoveTowards(transform.position, target.position, 50 * speed * Time.deltaTime);
        //transform.LookAt(target); 
    }
    private void FireCannon()
    {
        // Calculate direction towards target
        Vector3 direction = (target.transform.position - transform.position).normalized;
        // Calculate distance to target
        float distance = Vector3.Distance(transform.position, target.transform.position);

        // Calculate initial velocity and angle for cannonball
        float projectileAngle = 45f; 

        Vector3 projectileVelocity = Quaternion.AngleAxis(projectileAngle, transform.right) * transform.up * projectileSpeed;
        transform.rotation = Quaternion.LookRotation(direction);

        // Fire cannonball
        rb.velocity = projectileVelocity;
        rb.useGravity = true;
        rb.mass = 100;

        // Start descending when cannonball is close to target
        if (distance < 20f)
        {
            float timeToTarget = distance / projectileSpeed;
            float gravity = Physics.gravity.y;
            float initialVerticalVelocity = (target.transform.position.y - transform.position.y - 0.5f * gravity * timeToTarget * timeToTarget) / timeToTarget;

            Vector3 newProjectileVelocity = direction * projectileSpeed + Vector3.up * initialVerticalVelocity;
            rb.velocity = newProjectileVelocity;

            // Apply gravity to the cannonball
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (projectileType == ProjectileType.Arrow)
            {
                DealNormalDamage(other);
            }
            else if (projectileType == ProjectileType.Cannonball)
            {
                DealAOEDamage(tls.baseAoeRadius);
                //other.GetComponent<Rigidbody>().AddForce(forceDirection * tls.baseAoeBlastForce, ForceMode.Impulse);
            }
        } 
    }
    private void DealNormalDamage(Collider hit)
    {
        if (hit.TryGetComponent(out IDamageable targetDamagable))
        {
            targetDamagable.TakeDamage(gameObject, damage);
        }
        Destroy(gameObject);
    }
    private void DealAOEDamage(float aoe)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, aoe);
        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable targetDamagable))
            {
                targetDamagable.TakeDamage(gameObject, damage);
            }
        }
        Destroy(gameObject);
    }
}
