using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This script allows for us to click anywhere on the NavMesh and have our player move there.
/// </summary>
public class Player : MonoBehaviour
{
    // Temp 
    [Header("For Debugging")]
    public bool HaveTarget = false;
    public GameObject SelectedTarget;
    [SerializeField] private float manacost = 10f; 

    [Header("Player Stats")]
    [SerializeField] private float currentHealth; 
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentMana; 
    [SerializeField] private float maxMana;
    [SerializeField] private float clickDistance;

    public GameObject MySelectedTarget { get; set; }

    // temp magic spells array
    [Tooltip("Press 1,2,3 to cast dif elemental spell")]
    [SerializeField] private GameObject[] spellPrefab;
    [SerializeField] private Transform _magicCastExit;
    protected bool isAttacking = false;                 // can be used to activate animation for attack later on
    protected Coroutine attackRoutine;
    private NavMeshAgent playerAgent;
    //private SpellBook _spellBook;
    private MagicBook magicbook; 
    protected bool canMove => Input.GetMouseButton(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    // Player Stats region
    public CharacterStats characterStats;
    //public int power = 10, toughness = 10, atkSpd = 10;
    private Color hitColor = Color.yellow;

    //private EnemyTarget enemytarget; 
    void InitializeStats()
    {
        // Set player health
        maxHealth = 100f;
        currentHealth = maxHealth;

        // Set player mana
        maxMana = 50f; 
        currentMana = maxMana;

        // Set max click distance
        clickDistance = 300f;
        // Initialize player stats  
        //characterStats = new CharacterStats(power, toughness, atkSpd);
    }
    void Start()
    {
        InitializeStats();
        
        if (playerAgent == null)
            playerAgent = GetComponent<NavMeshAgent>();

        magicbook = GetComponent<MagicBook>(); 
        //EnemyTarget enemytarget = GetComponent<EnemyTarget>();
    }

    void Update()
    {
        MagicLineOfSight();
        RotateFollowMouse();
        // Move on mouse click 
        if (canMove)
        {
            MoveToCursor(); 
        }
        // Cancel/Detarget any selected target
        if (Input.GetKeyDown(KeyCode.Escape)) 
        { 
            MySelectedTarget = null; 
        }
        //Debug.DrawRay(transform.position, transform.forward * 5f, Color.red);

        #region FOR DEBUGGING
        SelectedTarget = MySelectedTarget; 

        if (MySelectedTarget == null)
            HaveTarget = false;
        else
            HaveTarget = true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestoreMana(50); 
        }

        #endregion
    }
    private void RotateFollowMouse()
    {
        //Vector3 mousePos = Input.mousePosition;
        //Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        //mousePos.z = 0;
        //mousePos.x = mousePos.x - objectPos.y;
        //mousePos.y = mousePos.y - objectPos.z;

        //float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(new Vector3(0, angle + 360, 0));

    }
    private void MoveToCursor()
    {
        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, clickDistance))
            {
                //GameObject interactedObject = hitInfo.collider.gameObject;
                if (hitInfo.collider != null)
                    MySelectedTarget = hitInfo.collider.gameObject;


                if (MySelectedTarget.tag == "Interactable Object")
                {
                    //Debug.Log(interactedObject.name); 
                    MySelectedTarget.GetComponent<Interactable>().MoveToInteraction(playerAgent);
                }
                else if (MySelectedTarget.tag == "Enemy")
                {
                    GetComponent<PlayerCombat>().SelectEnemy(MySelectedTarget.GetComponent<EnemyTarget>());
                    MySelectedTarget.GetComponent<Interactable>().MoveToInteraction(playerAgent);
                }
                else  // Target is non-interactable
                {
                    playerAgent.stoppingDistance = 0;
                    playerAgent.destination = hitInfo.point;
                }
            }
        }
    }

    //private void interactWithCombat()
    //{
    //    RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
    //    foreach (RaycastHit hit in hits)
    //    {
    //        EnemyTarget target = hit.transform.GetComponent<EnemyTarget>(); 
    //        if (target == null) continue;
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            SelectEnemy(target);
    //        }
    //    }
    //}
    public void SelfHealing(int amount)
    {
        // Add some hp to player
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
            UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }
    }
    public void RestoreMana(int amount)
    {
        // Add some mp to player
        if (currentMana < maxMana)
        {
            currentMana += amount;
            UIManager.Instance.UpdateManaBar(currentMana, maxMana);
            if (currentMana > maxMana)
                currentMana = maxMana;
        }
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
        if (currentHealth <= 0)
            Die(); 
    }
    private void Die()
    {
        Debug.Log("Player dead. Reset Health.");
        this.currentHealth = this.maxHealth; 
    }
    // How Enemies does damage to YOU!
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyTarget enemyTarget = other.gameObject.GetComponent<EnemyTarget>();
            int dmgAmt = enemyTarget.power;
            //Debug.Log("Received " + dmgAmt + " damage!");
            TakeDamage(dmgAmt);
        }
    }
    #region Magic Casting 
    private bool MagicLineOfSight()
    {
        Vector3 origin = transform.position;
        Vector3 dir = transform.forward;
        float rayLength = 50f;
        Debug.DrawRay(origin, dir * rayLength, Color.green);

        Ray ray = new Ray(origin, dir);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, rayLength, LayerMask.GetMask("Enemy")))
        {   // Wont fire spell at non-enemies
            if (hitInfo.collider.CompareTag("Enemy"))
                MySelectedTarget = hitInfo.transform.gameObject; 
                return true;
        }
        return false;
    }
    /// <summary>
    /// Cast spell by pressing 1-3 keys
    /// </summary>
    /// <param name="spellIndex"></param>
    public void CastSpell(int spellIndex)
    {
        //Debug.Log("Casted Spell!");

        if (MySelectedTarget != null && !isAttacking && MagicLineOfSight())
            attackRoutine = StartCoroutine(SpellAttack(spellIndex));
        
    }
    private void StopAttack()
    {
        // magicbook stopattack function
        magicbook.StopCasting();
        // player's own stopattack functions
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
            Debug.Log("Stopped casting");
        }
        
    }
    private IEnumerator SpellAttack(int spellIndex)
    {

        //Transform currentTarget = TargetEnemy;
        MagicData newSpell = magicbook.CastMagic(spellIndex);
        //Spells newSpell = _spellBook.CastSpell(spellIndex);
        
        // Here can optionally add a spell casttime if wanted
        //yield return new WaitForSeconds(3);

        if (MagicLineOfSight()) 
        {
            isAttacking = true;
            //CastSpell(0);
        }

        if (currentMana >= manacost)
        {
            currentMana -= manacost;
            UIManager.Instance.UpdateManaBar(currentMana, maxMana);

            //SpellScript s = Instantiate(spellPrefab[spellIndex], _magicCastExit.position, Quaternion.identity).GetComponent<SpellScript>();
            SpellScript s = Instantiate(newSpell.SpellPrefab, _magicCastExit.position, Quaternion.identity).GetComponent<SpellScript>();
            s.SpellTarget = MySelectedTarget.transform; 
            yield return new WaitForSeconds(newSpell.CastTime);
            StopAttack();

        }
        else  // Not enough mana!
        {
            Debug.Log("Insufficient Mana! ( Mana = " + currentMana + ")");
        }



        //yield return new WaitForSeconds(newSpell.CastTime); 

        //if (currentTarget != null)
        //{
        //    SpellScript s = Instantiate(newSpell.SpellPrefab, _magicCastExit.position, Quaternion.identity).GetComponent<SpellScript>();
        //    s.Init(currentTarget, newSpell.Damage);
        //}
        // Bind method to button
        //CastSpell();


    }
    #endregion
}
