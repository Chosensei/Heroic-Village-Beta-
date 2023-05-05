using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    private Transform target;
    private Vector3 initialPosition, targetPosition;
    private float speed;
    private float damage;
    private float aoeFxRadius;
    private float aoeImpactForce;
    private float effectDuration;
    private float damageOT;
    private float startTime;
    private bool hasInitialized = false;
    private Rigidbody rb;
    private Vector3 forceDirection;
    private TowerLevelSwitch tls; 
    public ProjectileType projectileType;
    public float launchForce;
    public float projectileSpeed = 20f;
    [Header("Cannon Tower Setting")]
    public float explosionRadius = 25;  // the radius of the explosion that deals damage to enemies
    public float cannonForce = 1000f; // the force with which the cannonball hits enemies
    [Header("FOR DEBUG ONLY")]
    public GameObject currentTarget = null;
    private void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>(); 
        // Auto destroy spell gameobject after 15s
        Destroy(this.gameObject, 15f);
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
                // Fire projectile in an arc trajectory manner
                FireCannon();
                // Old way
                //if (transform.position == targetPosition)
                //{
                //    DealAOEDamage(tls.baseAoeRadius);
                //}
                // New way
                if (Vector3.Distance(transform.position, targetPosition) < 0.25f)
                {
                    DealAOEDamage(aoeFxRadius);
                }
            }
            else if (projectileType == ProjectileType.Fireball || projectileType == ProjectileType.Iceball
                || projectileType == ProjectileType.Thunderball)
            {
                if (targetPosition == null) Destroy(gameObject);
                FireArrow();
                // Apply AOE damage upon collision (PROBLEM HERE)
                if (Vector3.Distance(transform.position, target.position) < 0.25f)
                {
                    // test
                    DealAOEDamage(aoeFxRadius);
                    //DealAOEDamage(5);
                    //DealAOEDamage(tls.baseAoeRadius);
                }
            }
            // check if the target is dead and destroy the projectile if it is
            //if (target.TryGetComponent(out EnemyBehavior enemy) && enemy.isDead)
            //{
            //    Destroy(gameObject);
            //    return;
            //}
        }
             
    }
    public void InitializeArrow(Transform target, float speed, float damage)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        startTime = Time.time;
        hasInitialized = true;
    }
    public void Initialize(Transform target, float speed, float damage, float aoeFxRadius, float aoeImpactForce)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.aoeFxRadius = aoeFxRadius;
        this.aoeImpactForce = aoeImpactForce;
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
    public void InitializeWizard(Transform target, float speed, float damage, float effectDuration, float damageOT)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.effectDuration = effectDuration;
        this.damageOT = damageOT; 
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
            rb.AddForce(Vector3.down * gravity , ForceMode.Acceleration);
        }
    }


    //protected virtual void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Enemy")
    //    {
    //        if (projectileType == ProjectileType.Arrow)
    //        {
    //            DealNormalDamage(other);
    //        }
    //        else if (projectileType == ProjectileType.Cannonball)
    //        {
    //            //DealAOEDamage(5); 
    //            DealAOEDamage(aoeFxRadius);
    //            other.GetComponent<Rigidbody>().AddForce(forceDirection * aoeImpactForce, ForceMode.Impulse);
    //        }
    //    } 
    //}
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && target != null)
        {
            if (projectileType == ProjectileType.Arrow)
            {
                DealNormalDamage(other);
            }
            else if (projectileType == ProjectileType.Cannonball)
            {
                //DealAOEDamage(5); 
                DealAOEDamage(aoeFxRadius);
                other.GetComponent<Rigidbody>().AddForce(forceDirection * aoeImpactForce, ForceMode.Impulse);
            }

            // Check if this is the first enemy hit
            //if (other.transform == target || targetPosition == other.transform.position)
            //{
            //    target = null;
            //    Destroy(gameObject);
            //}
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

                // Check for other special types projectiles
                if (projectileType == ProjectileType.Fireball)
                {
                    hit.GetComponent<EnemyBehavior>().FireBurn(effectDuration, damageOT);
                    Debug.Log("Fire hit!");
                }
                if (projectileType == ProjectileType.Iceball)
                {
                    Debug.Log("Ice hit!");
                    hit.GetComponent<EnemyBehavior>().IceSlow(effectDuration);
                }
                if (projectileType == ProjectileType.Thunderball)
                {
                    hit.GetComponent<EnemyBehavior>().ThunderStun(effectDuration, damageOT);
                }
            }
        }
        Destroy(gameObject);
    }
}
