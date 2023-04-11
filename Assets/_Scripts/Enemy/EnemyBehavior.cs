using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour, IDamageable
{
    [Header("FOR DEBUG ONLY")]
    public GameObject currentTarget;
    [Space]
    [Header("Target Search")]
    [SerializeField] private float searchRadius = Mathf.Infinity; // Search range of the AI
    [SerializeField] private LayerMask targetLayer; // Layer of the targets
    [SerializeField] private float deathDuration = 5f;
    [Space]
    [Header("Enemy SO")]
    public EnemyData enemyData;
    [SerializeField] private HealthBarController HealthBar; 
    private bool targetWithinRange => Vector3.Distance(transform.position, currentTarget.transform.position) <= enemyData.atkRange;
    public static bool isDead = false;
    private bool isAttacking = false;
    private float maxHealth, currentHealth; 
    private float attackRate, attackCooldown;  
    private List<GameObject> targetsInRange = new List<GameObject>();
    private NavMeshAgent agent;
    private Animator animator;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetupAIFromSOConfig(); 
    }
    void Start()
    {
        currentHealth = maxHealth;
        FindNewTargetWithArray();
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        //FOR DEBUGGING ONLY
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(null, 10);
            Debug.Log(currentHealth);
            //HealthBar.UpdateDamageUI(); 
        }

        if (isDead) return;

        if (isAttacking)
        {
            animator.SetBool("isWalking", false);
            agent.isStopped = true;
        }
        else
        {
            animator.SetBool("isWalking", true);
            agent.isStopped = false;
        }
            

        // If there is no current target or if the current target is inactive, find a new target 
        if (currentTarget == null || !currentTarget.activeInHierarchy)
        {
            FindNewTargetWithArray();
        }
        else
        {
            // If there is a current target and it is within attack range, attack it
            if (targetWithinRange)
            {
                if (!isAttacking)
                    StartCoroutine(Attack());
            }
            else 
            {
                isAttacking = false;
                agent.isStopped = false;
                agent.SetDestination(currentTarget.transform.position);
            }
        }

    }
    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("attack");

        yield return new WaitForSeconds(attackCooldown);

        if (currentTarget != null)
        {
            if (currentTarget.TryGetComponent(out IDamageable targetDamagable))
            {
                targetDamagable.TakeDamage(this.gameObject, CalculateDmg());
            }
        }
        isAttacking = false;
        animator.ResetTrigger("attack");
    }
    private void StartAttack()
    {
        isAttacking = true;
        agent.isStopped = true;
        animator.SetTrigger("attack");

        //IDamageable targetDamagable = currentTarget.GetComponent<IDamageable>();
        //if (targetDamagable != null)
        //{
        //    targetDamagable.TakeDamage(CalculateDmg());
        //}
        //if (currentTarget.TryGetComponent(out IDamageable targetDamagable))
        //{
        //    targetDamagable.TakeDamage(CalculateDmg());
        //}


        // Probably need to check if it's the player or building that we're attacking
        // in order to send the correct info to the health script underlying the object
        //currentTarget.GetComponent<BuildingHealth>().TakeDamage(attackDamage);
        //currentTarget.GetComponent<PlayerHealth>().TakeDamage(attackDamage);


    }
    private void EndAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
        //animator.ResetTrigger("attack");
    }
    private void FindNewTargetWithList()
    {
        // Clear previous targets list
        targetsInRange.Clear();

        // Find all targets within range and add them to the list
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius, targetLayer);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Target"))
            {
                targetsInRange.Add(collider.gameObject);
            }
        }
    }
    private void FindNewTargetWithArray()
    {
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, searchRadius, targetLayer);
        if (targetsInRange.Length > 0)
        {
            float closestDistance = Mathf.Infinity;
            foreach (Collider target in targetsInRange)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    currentTarget = target.gameObject;
                }
            }
        }
    }
    public void SetupAIFromSOConfig()
    {
        agent.speed = enemyData.moveSpeed;
        agent.stoppingDistance = enemyData.atkRange;
        attackRate = enemyData.atkSpeed; 
        attackCooldown = enemyData.timeBetweenAtks;
        maxHealth = enemyData.maxHealth;
        currentHealth = enemyData.currentHealth; 
    }

    #region Player detection logic zone

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentTarget = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentTarget = null;
        }
    }

    #endregion
    private int CalculateDmg()
    {
        return Random.Range(enemyData.minDamage, enemyData.maxDamage + 1);
    }
    public void TakeDamage(GameObject instigator, float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        //animator.SetTrigger("hit");
        if (currentHealth <= 0)
        {
            HealthBar.gameObject.SetActive(false); 
            Die();
        }
        // Update Health Bar UI 
        HealthBar.UpdateHealthBar(currentHealth, maxHealth);
    }
    private void Die()
    {
        isDead = true;
        animator.SetTrigger("death");
        agent.ResetPath();
        //AudioSource.PlayClipAtPoint(deathSound, transform.position);
        agent.isStopped = true;
        Destroy(gameObject, deathDuration);
    }
    // Draw the search radius in editor
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, searchRadius);
    //}
}
