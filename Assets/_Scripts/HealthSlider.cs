using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;

public class HealthSlider : MonoBehaviour
{
    Health health;
    Image healthBarFill;
    private void Awake()
    {
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
        healthBarFill = GetComponent<Image>();
        //healthBarFill.fillAmount = 1; 
    }

    private void Update()
    {
        float healthPercentage = health.GetFraction();
        healthBarFill.fillAmount = healthPercentage;
    }

}