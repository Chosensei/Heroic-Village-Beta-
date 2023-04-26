using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    //public static bool isDead = false;
    public bool isDead = false;
    private bool isAttacking = false;
    private float maxHealth, currentHealth; 
    private float attackRate, attackCooldown;  
    private List<GameObject> targetsInRange = new List<GameObject>();
    private NavMeshAgent agent;
    private Animator animator;
    private Renderer renderer;
    private float remainingBurnTime, remainingSlowTime, remainingStunTime;

    // Wall
    public GameObject[] CageWall;
    public int wallIndex;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        renderer = transform.GetChild(0).GetComponent<Renderer>();
        SetupAIFromSOConfig(); 
    }
    void Start()
    {
        currentHealth = maxHealth;
        FindNewTargetWithArray();
        animator.SetBool("isWalking", true);

        // Add walls
        int numWalls = 3;
        CageWall = new GameObject[numWalls];
        // Find all wall objects and add them to the array
        for (int i = 0; i < numWalls; i++)
        {
            CageWall[i] = GameObject.FindWithTag("Wall " + i);
        }
        // Set the current wall index variable to the number of wall objects that could be found
        wallIndex = CageWall.Length - 1;
    }

    void Update()
    {
        //FOR DEBUGGING ONLY
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(null, 10);
            //Debug.Log(currentHealth);
            HealthBar.UpdateHealthBar(currentHealth, maxHealth);
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
        //Fire Burn DOT
        //if (remainingBurnTime > 0)
        //{
        //    remainingBurnTime -= Time.deltaTime;
        //}
        //// Ice Slow
        //if (remainingSlowTime > 0)
        //{
        //    agent.speed = enemyData.moveSpeed / 2;
        //    remainingSlowTime -= Time.deltaTime;
        //}
        //else
        //{
        //    agent.speed = enemyData.moveSpeed;
        //}
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
            else if (currentTarget.TryGetComponent(out CageWall cw))
            {

                cw.TakeDamage(CalculateDmg(), wallIndex);
                cw.isUnderAtk = true;

                // Find the index of the destroyed wall in the CageWall array
                int index = Array.IndexOf(CageWall, cw.gameObject);
                Debug.Log(index);
                if (cw.isDestroyed)
                {
                    // Disable the wall layer 
                    CageWall[wallIndex].SetActive(false);
                    // Disable destroyed wall HP UI 
                    UIManager.Instance.WallUIObjects[wallIndex].SetActive(false);

                }
            }
        }
        isAttacking = false;
        animator.ResetTrigger("attack");
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
        return UnityEngine.Random.Range(enemyData.minDamage, enemyData.maxDamage + 1);
    }
    /// <summary>
    /// Interface method
    /// </summary>
    /// <param name="instigator"></param>
    /// <param name="damage"></param>
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
    public void Die()
    {
        isDead = true;
        animator.SetTrigger("death");
        agent.ResetPath();
        //AudioSource.PlayClipAtPoint(deathSound, transform.position);
        agent.isStopped = true;
        Destroy(gameObject, deathDuration);
    }
    private IEnumerator DamageOverTime(float duration, float damagePerTick)
    {
        float timeRemaining = duration;
        Color originalColor = renderer.material.color;  
        while (timeRemaining > 0)
        {
            Debug.Log("DOT trigger");
            currentHealth -= damagePerTick;
            renderer.material.color = Color.red; // Change the material color to red
            //animator.SetTrigger("hit");
            if (currentHealth <= 0)
            {
                Debug.Log("HP reduced!");
                HealthBar.gameObject.SetActive(false);
                Die();
                yield break;
            }
            // Update Health Bar UI 
            HealthBar.UpdateHealthBar(currentHealth, maxHealth);
            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        renderer.material.color = originalColor; // Restore the original material color
    }
    private IEnumerator ApplySlowEffect(float duration)
    {
        float originalSpeed = agent.speed; // Store the original movement speed
        Color originalColor = renderer.material.color;  // Store the original color
        agent.speed /= 2f; // Reduce the movement speed by half
        renderer.material.color = Color.blue; // Change the material color to blue
        yield return new WaitForSeconds(duration); // Wait for the slow effect duration to expire
        agent.speed = originalSpeed; // Restore the original movement speed
        renderer.material.color = originalColor; // Restore the original material color
    }
    private IEnumerator ApplyStunEffect(float duration, float dot, GameObject magicCirclePrefab)
    {
        // Apply damage over Time
        StartCoroutine(DamageOverTime(duration, dot));
        GameObject stunMagicCircle = Instantiate(magicCirclePrefab, transform.position, Quaternion.identity);

        float originalSpeed = agent.speed; // Store the original movement speed
        Color originalColor = renderer.material.color;  // Store the original color
        agent.speed = 0; // Halt movement speed 
        renderer.material.color = Color.yellow; // Change the material color to blue
        yield return new WaitForSeconds(duration); // Wait for the stun effect duration to expire
        agent.speed = originalSpeed; // Restore the original movement speed
        renderer.material.color = originalColor; // Restore the original material color
    }
    public void FireBurn(float duration, float damagePerTick)
    {
        if (isDead) return;
        StartCoroutine(DamageOverTime(duration, damagePerTick));
    }
    public void IceSlow(float slowTime)
    {
        if (isDead) return;
        StartCoroutine(ApplySlowEffect(slowTime));
    }
    public void ThunderStun(float stunTime, float damage, GameObject spawnObj)
    {
        if (isDead) return;
        StartCoroutine(ApplyStunEffect(stunTime, damage, spawnObj));
    }

    private IEnumerator SlowRoutine(float time)
    {
        while (remainingSlowTime > 0)
        {
            agent.speed = enemyData.moveSpeed / 2;
            remainingSlowTime -= Time.deltaTime;
        }
        //slowed = null; 
        yield return null; 
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
    // Draw the search radius in editor
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, searchRadius);
    //}

    // IceSlow
    //remainingSlowTime += slowTime;
    //if (slowed == null)
    //{
    //    slowed = StartCoroutine(SlowRoutine(slowTime)); 
    //}
    //else
    //{
    //    agent.speed = enemyData.moveSpeed;
    //}

}
