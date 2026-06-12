using TMPro;
using UnityEngine;

public class IngredientInputData : MonoBehaviour
{
    public TMP_InputField quantityInput;
    public TMP_InputField unitInput;
    public TMP_InputField nameInput;
    public TMP_InputField customQuantityInput;
    public Ingredient selectedIngredient;
    

    public void DeleteIngredient()
    {
        Destroy(gameObject);
    }
}