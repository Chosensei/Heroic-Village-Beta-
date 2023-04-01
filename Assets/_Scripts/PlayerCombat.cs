using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class PlayerCombat : MonoBehaviour
{
    Player _player;
    protected bool isAttacking = false;    
    bool isCooldown = false;
    // can be used to activate animation for attack later on
    protected Coroutine attackRoutine;
    private MagicBook magicbook;
    // temp magic spells array
    [Tooltip("Press 1,2,3 to cast dif elemental spell")]
    [SerializeField] private GameObject[] spellPrefab;
    [SerializeField] private Transform _magicCastExit;

    void Start()
    {
        _player = GetComponent<Player>();
        magicbook = GetComponent<MagicBook>();
    }

    public void SelectEnemy(EnemyTarget target)
    {
        //Debug.Log("Enemy selected");
    }

    public void autoAttack(EnemyTarget target)
    {
        Debug.Log("Started attacking!");
        //bool isInRange = Vector3.Distance(transform.position, _player.SelectEnemy.transform.position);
    }

    #region Magic Casting 
    public bool MagicLineOfSight()
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
                _player.MySelectedTarget = hitInfo.transform.gameObject;
            return true;
        }
        return false;
    }
    /// <summary>
    /// For button Binds: Cast spell by pressing number keys (1-3) 
    /// </summary>
    /// <param name="spellIndex"></param>
    public void CastSpell(int spellIndex)
    {
        //Debug.Log("Casted Spell!");

        if (_player.MySelectedTarget != null && !isAttacking && MagicLineOfSight())
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
            //Debug.Log("Stopped casting");
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

        if (_player.MyCurrentMana >= _player.ManaCost)
        {
            _player.MyCurrentMana -= _player.ManaCost;
            UIManager.Instance.UpdateManaBar(_player.MyCurrentMana, _player.MyMaxMana);

            //SpellScript s = Instantiate(spellPrefab[spellIndex], _magicCastExit.position, Quaternion.identity).GetComponent<SpellScript>();
            SpellScript s = Instantiate(newSpell.SpellPrefab, _magicCastExit.position, Quaternion.identity).GetComponent<SpellScript>();
            // Activate skill cooldown UI Timer
            //SkillCooldownTimer(UIManager.Instance.castTimeImg[spellIndex], newSpell.CastTime, spellIndex);

            //if (UIManager.Instance.castTimeImg[spellIndex])
            //{
            //    Debug.Log("Now casting" + spellIndex);
            //}
            s.SpellTarget = _player.MySelectedTarget.transform;
            yield return new WaitForSeconds(newSpell.CastTime);
            StopAttack();

        }
        else  // Not enough mana!
        {
            Debug.Log("Insufficient Mana! ( Mana = " + _player.MyCurrentMana + ")");
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
