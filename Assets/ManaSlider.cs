using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI; 

public class ManaSlider : MonoBehaviour
{
    Mana mana;
    Image manaBarFill;
    private void Awake()
    {
        mana = GameObject.FindWithTag("Player").GetComponent<Mana>();
        manaBarFill = GetComponent<Image>();
        manaBarFill.fillAmount = 1; 
    }

    private void Update()
    {
        float manaPercentage = mana.GetFraction();
        manaBarFill.fillAmount = manaPercentage;
    }
}
