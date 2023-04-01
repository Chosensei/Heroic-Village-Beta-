using UnityEngine;
using UnityEngine.UI;

public class PageSwitcher : MonoBehaviour
{
    public GameObject[] pages;
    private int currentPageIndex = 0;

    void Start()
    {
        ShowCurrentPage();
    }

    public void ShowPreviousPage()
    {
        currentPageIndex--;
        if (currentPageIndex < 0)
        {
            currentPageIndex = pages.Length - 1;
        }
        ShowCurrentPage();
    }

    public void ShowNextPage()
    {
        currentPageIndex++;
        if (currentPageIndex >= pages.Length)
        {
            currentPageIndex = 0;
        }
        ShowCurrentPage();
    }

    private void ShowCurrentPage()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (i == currentPageIndex)
            {
                pages[i].SetActive(true);
            }
            else
            {
                pages[i].SetActive(false);
            }
        }
    }
}
