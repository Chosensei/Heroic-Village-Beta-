using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : MonoBehaviour, IAction
{
    [SerializeField] float autoAttackRange = 4f;
    [SerializeField] float actualAttackRange = 1f;
    [SerializeField] float timeBetweenAttacks = 1f;

    CageWall target;
    float timeSinceLastAttack = Mathf.Infinity;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        if (target.IsDead())
        {
            target = FindNewTargetInRange();
            if (target == null) return;
        }

        if (!GetIsInRange(target.transform))
        {
            GetComponent<AIMover>().MoveTo(target.transform.position, 1f);
        }
        else
        {
            GetComponent<AIMover>().Cancel();
            AttackBehaviour();
        }
    }
    public CageWall GetTarget()
    {
        return target;
    }
    private bool GetIsInRange(Transform targetTransform)
    {
        return Vector3.Distance(transform.position, targetTransform.position) < actualAttackRange;
        // if enemies has weapon in hand (if the distance between the target is less than the weapon range) 
        //return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetRange();
    }
    private CageWall FindNewTargetInRange()
    {
        print("Find New Target In Range");
        CageWall best = null;
        float bestDistance = Mathf.Infinity;
        foreach (var candidate in FindAllTargetsInRange())
        {
            float candidateDistance = Vector3.Distance(
                transform.position, candidate.transform.position);
            if (candidateDistance < bestDistance)
            {
                best = candidate;
                bestDistance = candidateDistance;
            }
        }
        return best;
    }

    private IEnumerable<CageWall> FindAllTargetsInRange()
    {
        print("Find All Target In Range");
        RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position,
                                            autoAttackRange, Vector3.up);
        foreach (var hit in raycastHits)
        {
            CageWall health = hit.transform.GetComponent<CageWall>();
            if (health == null) continue;
            if (health.IsDead()) continue;
            if (health.gameObject == gameObject) continue;
            yield return health;
        }
    }
    private void AttackBehaviour()
    {
        transform.LookAt(target.transform);
        if (timeSinceLastAttack > timeBetweenAttacks)
        {
            // This will trigger the Hit() event.
            TriggerAttack();
            timeSinceLastAttack = 0;
        }
    }
    private void TriggerAttack()
    {
        GetComponent<Animator>().ResetTrigger("stopAttack");
        GetComponent<Animator>().SetTrigger("attack");
    }
    private void StopAttack()
    {
        GetComponent<Animator>().ResetTrigger("attack");
        GetComponent<Animator>().SetTrigger("stopAttack");
    }
    public void Cancel()
    {
        StopAttack();
        target = null;
        GetComponent<AIMover>().Cancel();
    }

}
