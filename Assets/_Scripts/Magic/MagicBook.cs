using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
public class MagicBook : MonoBehaviour
{
    [SerializeField] MagicData[] magicData;
    //[SerializeField] Text[] skillTxt;
    //[SerializeField] Text[] castTimeTxt;
    //[SerializeField] Image[] castTimeImg;
    // reset HUD
    //UIManager.Instance.ResetHUDText(spellIndex);
    private Coroutine _spellRoutine;
    private bool doneCasting = false; 
    public MagicData CastMagic(int index)
    {
        //UIManager.Instance.skillTxt[index].text = magicData[index].Name;
        //UIManager.Instance.skillTxt[index].color = Color.white;
        //UIManager.Instance.castTimeTxt[index].color = Color.white;
        //UIManager.Instance.castTimeImg[index].color = new Color(0, 0, 0, 134); // light black color
        //UIManager.Instance.castTimeImg[index].fillAmount = 0;

        _spellRoutine = StartCoroutine(SpellCastDuration(index));

        return magicData[index];
    }
    private IEnumerator SpellCastDuration(int index)
    {
        float timePassed = Time.deltaTime;
        float rate = 1.0f / magicData[index].CastTime;
        float progress = 0.0f;

        while (progress <= 1.0f)
        {
            //UIManager.Instance.castTimeImg[index].fillAmount = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            timePassed += Time.deltaTime;
            // Display number of seconds passed after casting
            //UIManager.Instance.castTimeTxt[index].text = (magicData[index].CastTime - timePassed).ToString("F1");
            doneCasting = true;

            yield return null;
        }
        StopCasting(); 
        //if (doneCasting)
        //    UIManager.Instance.ResetHUDText(index);
    }
    public void StopCasting()
    {
        if (_spellRoutine != null)
        {
            StopCoroutine(_spellRoutine);
            _spellRoutine = null; 
        }
    }
}
