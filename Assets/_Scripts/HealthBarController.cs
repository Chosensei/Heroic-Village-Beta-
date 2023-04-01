using UnityEngine;
using UnityEngine.UI;
public class HealthBarController : MonoBehaviour
{
    public Image healthBar;  // Reference to the health bar UI element

    public void UpdateHealthBar(float currentHP, float maxHP)
    {
        float healthPercentage = currentHP / maxHP;
        healthBar.fillAmount = healthPercentage;   // Update health bar UI value to reflect current health percentage
    }
}
