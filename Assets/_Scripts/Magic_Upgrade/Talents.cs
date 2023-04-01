using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Talents : MonoBehaviour
{
    private Image sprite;
    [SerializeField] private Text countText;
    private void Awake()
    {
        sprite = GetComponent<Image>();
    }
    // Gray out areas of magic tier not yet unlocked
    public void Lock()
    {
        sprite.color = Color.gray;
        countText.color = Color.gray; 
    }
    // Reset colors of unlocked areas of magic tier
    public void Unlock()
    {
        sprite.color = Color.white;
        countText.color = Color.black;
    }
}
