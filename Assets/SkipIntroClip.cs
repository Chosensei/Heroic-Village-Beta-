using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SkipIntroClip : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ChangeSkipButtonTextAfterDelay());
    }
    public void SkipCutScene()
    {
        SceneManager.LoadScene("GameWorld");
    }
    private IEnumerator ChangeSkipButtonTextAfterDelay()
    {
        yield return new WaitForSeconds(25f);
        SceneManager.LoadScene("GameWorld");
    }
}
