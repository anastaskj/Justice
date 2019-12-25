using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageController : MonoBehaviour
{
    [SerializeField]
    Text pageText;

    GameObject activePage;
    GameObject nextPage;
    GameObject previousPage;

    public List<GameObject> UIpages = new List<GameObject>();

    private void Start()
    {
        SetActivePage(UIpages[0]);
    }

    private GameObject GetPreviousPage(GameObject page)
    {
        int index = UIpages.IndexOf(page);
        if (index - 1 > -1)
            return UIpages[index - 1];
        else
            return UIpages[UIpages.Count-1];
        
    }

    private GameObject GetNextPage(GameObject page)
    {
        int index = UIpages.IndexOf(page);
        if (index + 1 < UIpages.Count)
            return UIpages[index + 1];
        else
            return UIpages[0];
    }

    private void SetActivePage(GameObject page)
    {
        activePage = page;
        page.SetActive(true);

        previousPage = GetPreviousPage(page);

        if (previousPage != null)
        {
            previousPage.SetActive(false);
        }
        
        nextPage = GetNextPage(page);

        if (nextPage != null)
        {
            nextPage.SetActive(false);
        }

        pageText.text = (UIpages.IndexOf(activePage) + 1) + "/" + UIpages.Count;
    }
    
    public void GoToNextPage()
    {
        SetActivePage(GetNextPage(activePage));
    }

    public void GoToPrevPage()
    {
        SetActivePage(GetPreviousPage(activePage));
    }
   
}
