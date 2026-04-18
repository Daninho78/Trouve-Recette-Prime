using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigation : MonoBehaviour
{
    public GameObject panelHome;
    public GameObject panelLibrary;
    public GameObject panelRecipes;
    public GameObject panelRecipesDetails;
    public GameObject miniPanelAdd;
    public GameObject panelAddBook;
    public GameObject panelAddRecipe;
    public GameObject panelSearch;
   

    void CloseAllPanels()
    {
        panelHome.SetActive(false);
        panelLibrary.SetActive(false);
        panelRecipes.SetActive(false);
        panelRecipesDetails.SetActive(false);
        miniPanelAdd.SetActive(false);
        panelAddBook.SetActive(false);
        panelAddRecipe.SetActive(false);
        panelSearch.SetActive(false);
        
    }

    public void OpenHome()
    {
        CloseAllPanels();
        panelHome.SetActive(true);
    }

    public void OpenLibrary()
    {
        CloseAllPanels();
        panelLibrary.SetActive(true);
    }

    public void ToggleMenuAdd()
    {
        miniPanelAdd.SetActive(!miniPanelAdd.activeSelf);
    }

    public void OpenAddBook()
    {
        CloseAllPanels();
        panelAddBook .SetActive(true);
    }

    public void openSearch()
    {
        CloseAllPanels ();
        panelSearch .SetActive(true);
    }


    
}
