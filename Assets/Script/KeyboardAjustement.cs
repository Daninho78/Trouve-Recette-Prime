using UnityEngine;
using UnityEngine.UI;

public class KeyboardAdjuster : MonoBehaviour
{
    public LayoutElement keyboardSpacer;

    public float keyboardSpaceHeight = 350f;

    public bool forceKeyboardForEditor = false;

    public RectTransform panelAddRecipe;
    public RectTransform ingredientsContainer;


    private void Start()
    {
        DebugMeasureIngredientDistance();
    }
    void Update()
    {
        

        if (TouchScreenKeyboard.visible || forceKeyboardForEditor)
        {
            keyboardSpacer.preferredHeight = keyboardSpaceHeight;
        }
        else
        {
            keyboardSpacer.preferredHeight = 0f;
        }

    }

    public void DebugMeasureIngredientDistance()
    {
        if (panelAddRecipe == null || ingredientsContainer == null)
            return;

        Vector3[] panelCorners = new Vector3[4];
        Vector3[] ingredientCorners = new Vector3[4];

        panelAddRecipe.GetWorldCorners(panelCorners);
        ingredientsContainer.GetWorldCorners(ingredientCorners);

        float panelTop = panelCorners[1].y;
        float ingredientTop = ingredientCorners[1].y;

        float distance = panelTop - ingredientTop;

        Debug.Log("Distance Panel haut -> Ingredients haut : " + distance);
    }
}