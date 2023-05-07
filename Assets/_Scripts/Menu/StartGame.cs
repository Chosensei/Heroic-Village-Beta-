using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayMusic("Main_Menu"); 
    }
    public void LoadScene()
    {
        SceneManager.LoadScene("IntroductionScene");
    }
}
