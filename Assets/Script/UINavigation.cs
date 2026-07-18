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
    public GameObject panelFilter;
    public GameObject panelAddIngredients;
    public GameObject panelOCRReview;
   

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
        panelFilter.SetActive(false);
        panelAddIngredients.SetActive(false);
        panelOCRReview.SetActive(false);
        
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
        FindObjectOfType<BookListUI>().RefreshBooks();
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

    public void openFilterSearch()
    {
        panelFilter .SetActive(!panelFilter.activeSelf);
    }

    public void openAddIngredients()
    {
        panelAddIngredients .SetActive(true);
    }

    public void closeAddIngredients()
    {
        panelAddIngredients.SetActive(false);
    }
    
}
