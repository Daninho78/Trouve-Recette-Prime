using TMPro;
using UnityEngine;

public class IngredientItemUI : MonoBehaviour
{
    public TMP_Dropdown unitDropdown;
    public GameObject customQuantityInput;

    public void OnUnitChanged()
    {
        string selected = unitDropdown.options[unitDropdown.value].text;
        customQuantityInput.SetActive(selected == "Autre");
    }
}