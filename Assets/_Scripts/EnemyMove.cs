using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public Animator animator;
    public GameObject player; 
    public Transform defaultTarget;
    public float lookRadius = 10f;
    public float attackDistance = 2f;
    public float attackDelay = 1f;
    public float attackDamage = 10f;
    public float deathDuration = 2f;
    //public AudioClip attackSound;
    //public AudioClip deathSound;

    private NavMeshAgent agent;
    private Transform target;
    private bool isDead = false;
    private bool isAttacking = false;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = defaultTarget;
        agent.SetDestination(target.position);
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        if (isDead)
            return;

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            target = player.transform;
            agent.SetDestination(target.position);

            if (distance <= attackDistance && Time.time > lastAttackTime + attackDelay)
            {
                Attack();
            }
        }
        else
        {
            target = defaultTarget;
            agent.SetDestination(target.position);
        }

        if (distance <= attackDistance && !isAttacking)
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            animator.SetTrigger("attack");
            isAttacking = true;
        }
        else if (distance > attackDistance && isAttacking)
        {
            //agent.isStopped = false;
            //animator.SetBool("isWalking", true);
            EndAttack();
            //isAttacking = false;
        }
    }
    void Attack()
    {
        lastAttackTime = Time.time;
        isAttacking = true;
        agent.isStopped = true;
        animator.SetTrigger("attack");

        //AudioSource.PlayClipAtPoint(attackSound, transform.position);
        //target.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
    }

    public void EndAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
        animator.SetBool("isWalking", true);

    }

    //public void TakeDamage(float damage)
    //{
    //    if (isDead)
    //        return;

    //    animator.SetTrigger("hit");

    //    if (GetComponent<EnemyHealth>().TakeDamage(damage))
    //    {
    //        isDead = true;
    //        animator.SetTrigger("death");
    //        AudioSource.PlayClipAtPoint(deathSound, transform.position);
    //        agent.isStopped = true;
    //        Destroy(gameObject, deathDuration);
    //    }
    //}
}
