using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class UnitSuggestionManager : MonoBehaviour
{
    private List<Unit> allUnits = new List<Unit>();

    private async void Start()
    {
        await LoadUnits();
    }

    private async Task LoadUnits()
    {
        allUnits = await UnitService.GetAllUnits();
        Debug.Log("Suggestions unit�s pr�tes : " + allUnits.Count);
    }

    public List<Unit> GetSuggestions(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return new List<Unit>();

        string search = searchText.ToLower().Trim();

        return allUnits
            .Where(u => u.Name.ToLower().Contains(search))
            .OrderBy(u => u.Name.ToLower().StartsWith(search) ? 0 : 1)
            .ThenBy(u => u.Name)
            .Take(10)
            .ToList();
    }

    public Unit FindExactUnit(string unitName)
    {
        if (string.IsNullOrWhiteSpace(unitName))
            return null;

        string search = unitName.ToLower().Trim();

        return allUnits.FirstOrDefault(u =>
            u.Name.ToLower().Trim() == search
        );
    }
}