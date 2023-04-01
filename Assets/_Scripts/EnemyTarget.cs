using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyTarget : Interactable, IEnemy
{
    #region variables 
    // Temp 
    [Header("For Debugging")]
    public GameObject defaultTarget;
    public bool HaveTarget = false;
    public GameObject CurrentTarget;

    [Header("General Stats Variables")]
    public float currentHealth;
    public float maxHealth = 10;
    public float moveSpeed = 5;
    public int power = 5, toughness = 10, atkSpd = 2, atkRange = 1;
    public GameObject AttackTarget { get; set; }
    public bool isAlive { get; set; } = true; 
    const string TARGETTAG = "Cage Wall";
    //GameObject spawner; 
    #endregion

    #region references
    // Target detection 
    /* Temporary assign Walls directly in inspector */
    [SerializeField] private GameObject[] CageWallTarget = new GameObject[3];
    public int wallTargetIndex;
    [SerializeField] List<GameObject> wallTargetList = new List<GameObject>();

    private CharacterStats characterStats;
    private NavMeshAgent navAgent;
    private Rigidbody rb; 
    private Player player;
    [SerializeField] private Image enemyHealthbar;
    [SerializeField] private Transform canvasTransform;

    // Visual Feedback
    //private Renderer _renderer;
    //private Color originalColor;
    //private float damageFlashTime = .15f;
    #endregion
    void Start()
    {
        // Set first wall layer as initial default target (value Set inside CageWall)
        CageWallTarget[0] = GameObject.FindWithTag("Wall 0");
        CageWallTarget[1] = GameObject.FindWithTag("Wall 1");
        CageWallTarget[2] = GameObject.FindWithTag("Wall 2");
        wallTargetIndex = 2;
        defaultTarget = CageWallTarget[wallTargetIndex];
        // Find spawner 
        //spawner = GameObject.Find("Spawner 1");

        // Initialize values
        currentHealth = maxHealth;
        //originalColor = _renderer.material.color;

        // References
        //_renderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();

        // Find all targets
        wallTargetList = GameObject.FindGameObjectsWithTag(TARGETTAG).ToList();
        // Initialize enemy stats this way  
        //characterStats = new CharacterStats(power, toughness, atkSpd);

    }
    private void Update()
    {
        #region FOR DEBUGGING

        CurrentTarget = AttackTarget;

        if (AttackTarget == null)
            HaveTarget = false;
        else
            HaveTarget = true;

        #endregion
        defaultTarget = AttackTarget;

        // Set target to closest target
        if (AttackTarget == null)
        {
            AttackTarget = FindClosestTarget();
        }
        else
            MoveToTarget();

        //if (!isAlive)
        //    spawner.GetComponent<Spawner>().enemyCount--; 

        if (defaultTarget == null)
        {
            defaultTarget = CageWallTarget[wallTargetIndex];

            switch (wallTargetIndex)
            {
                case 0: // Last Wall index
                    defaultTarget = CageWallTarget[0];
                    break;

                case 1: // Second Wall index
                    defaultTarget = CageWallTarget[1];
                    break;

                case 2: // First Wall index
                    defaultTarget = CageWallTarget[2];
                    break;

                default:
                    print("No target!");
                    return;
            }
        }
        // Nothing in it
        if (Input.GetKeyDown(KeyCode.T))
        {

        }
    }
    private void FixedUpdate()
    {
        //withinAggroColliders = Physics.OverlapSphere(transform.position, aggroRadius, aggroLayerMask);
        //if (withinAggroColliders.Length > 0)
        //{
        //    //Debug.Log("Player in range!");
        //    ChasePlayer(withinAggroColliders[0].GetComponent<Player>());
        //}
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    TakeDamage(6);
        //}
    }
    private void LateUpdate()
    {
        // Update Ui elements in worldspace canvas to always face camera 
        canvasTransform.LookAt(transform.position + Camera.main.transform.forward); 
    }
    private void MoveToTarget()
    {
        navAgent.SetDestination(AttackTarget.transform.position);
        navAgent.stoppingDistance = 5;
    }
    public GameObject FindClosestTarget()
    {
        GameObject closest = gameObject;
        float leastDist = Mathf.Infinity;

        // Get distance of all targets from enemy and check whose least is the closest one
        foreach (var target in wallTargetList)
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
  
    public void ResetTargetList()
    {
        for (int i = 0; i < wallTargetList.Count; i++)
        {
            wallTargetList.Remove(FindClosestTarget());
            //Debug.Log("Wall removed from list!");
        }
    }
    public void ResetNewTarget()
    {
        AttackTarget = null;
        wallTargetList = GameObject.FindGameObjectsWithTag(TARGETTAG).ToList();
        //Debug.Log("Target Reset Successful!");
    }
    private void UpdateHealthBar()
    {
        enemyHealthbar.fillAmount = currentHealth / maxHealth;
    }
    public override void Interact()
    {
        Debug.Log("Enemy selected");
    }

    // Not necessary right now, since enemy damage is received inside SpellScript (collision trigger-based) 
    public void TakeDamage(float amount)
    {
        // Deal damage 
        //StartCoroutine(DamageVisualFeedback());
        currentHealth = currentHealth - amount;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }
    // How Enemies does damage to Wall
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out CageWall cw))
        {
            cw.TakeDamage(power, wallTargetIndex);
            cw.isUnderAtk = true;
            if (cw.isDestroyed)
            {
                //Destroy(CageWallTarget[wallTargetIndex].gameObject);
                CageWallTarget[wallTargetIndex].gameObject.SetActive(false);
                wallTargetIndex--;
                ResetTargetList();
                ResetNewTarget();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CageWall cw))
        {
            //Debug.Log("Out of Atk Range!");
            cw.isUnderAtk = false;
            if (cw.isDestroyed)
            {
                wallTargetIndex--;
                ResetTargetList();
                ResetNewTarget();
            }
        }
    }
    private IEnumerator Die()
    {
        isAlive = false;
        yield return new WaitForSeconds(1.5f);
        //Debug.Log("Enemy Died!");
        Destroy(this.gameObject);
    }
    //if (navAgent.remainingDistance <= navAgent.stoppingDistance)
    //{

    //}
    //private IEnumerator DamageVisualFeedback()
    //{
    //    _renderer.material.color = Color.white;
    //    yield return new WaitForSeconds(damageFlashTime);
    //    _renderer.material.color = originalColor;
    //}


    #region AI specific behaviors implementation
    //void ChasePlayer(Player player)
    //{
    //    // This line sets player as default target
    //    navAgent.SetDestination(player.transform.position);
    //    this.player = player;
    //    if (navAgent.remainingDistance <= navAgent.stoppingDistance)
    //    {
    //        if (!IsInvoking("PerformAttack"))
    //            InvokeRepeating("PerformAttack", .5f, 3f);
    //    }
    //    else
    //    {
    //        CancelInvoke("PerformAttack");
    //    }
    //}
    #endregion
}
