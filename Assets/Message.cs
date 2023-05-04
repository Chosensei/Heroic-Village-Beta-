using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.UI;
using TMPro; 
public class Message : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = Color.yellow; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = Color.white; 
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
