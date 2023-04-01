using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpellBook : MonoBehaviour
{
    [SerializeField] Spells[] spells;
    [SerializeField] Text[] castTime;

    [SerializeField] Image[] icon;

    private Coroutine _spellRoutine;
    public Spells CastSpell(int index)
    {

        icon[index].fillAmount = 0;

        _spellRoutine = StartCoroutine(SpellCastDuration(index));
        return spells[index];
    }

    private IEnumerator SpellCastDuration(int index)
    {
        float timePassed = Time.deltaTime;
        float rate = 1.0f / spells[index].CastTime;
        float progress = 0.0f;

        while (progress <= 1.0f)
        {
            icon[index].fillAmount = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            timePassed += Time.deltaTime;

            castTime[index].text = (spells[index].CastTime - timePassed).ToString("F1");
            yield return null;
        }
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
