using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoblin : MonoBehaviour, IEnemy
{
    private Renderer _renderer;
    private Color _originalColor;
    private float _damageFlashTime = .15f;
    private GameObject[] _enemySpawner;
    private EnemySpawner spawner;

    public float curHealth;
    public float maxHealth = 100;
    public float power = 1, toughness; 


    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
        _enemySpawner = GameObject.FindGameObjectsWithTag("EnemySpawners");
        //spawner.GetComponent<EnemySpawner>();
        curHealth = maxHealth; 
    }

    
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            StartCoroutine(DamageVisualFeedback());
            //curHealth -= damage;
            //if (curHealth <= 0)
            //{
            //  Die();
            //  SubtractFromSpawner();
            //}
        }
    }
    private IEnumerator DamageVisualFeedback()
    {
        _renderer.material.color = Color.white;
        yield return new WaitForSeconds(_damageFlashTime);
        _renderer.material.color = _originalColor;
    }
    void SubtractFromSpawner()
    {
        spawner.enemiesSpawned--;
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("curHealth before damage" + curHealth);

        curHealth -= amount;
        Debug.Log("curHealth after damage" + curHealth);
        if (curHealth <= 0)
        {
            Die();
        }
    }

    public void PerformAttack()
    {
        throw new System.NotImplementedException();
    }
    private void Die()
    {
        Destroy(this.gameObject);
    }
}
