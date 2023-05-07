using UnityEngine;
using UnityEngine.SceneManagement;
public class EndingBackToMainMenu : MonoBehaviour
{
    private float timer = 0f;
    private float waitTime = 25f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
