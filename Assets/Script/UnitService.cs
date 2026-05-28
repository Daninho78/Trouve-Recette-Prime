using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UnitService
{
    public static async Task<List<Unit>> GetAllUnits()
    {
        try
        {
            var result = await Supabase.Client.Instance
                .From<Unit>()
                .Get();

            return result.Models;
        }
        catch (Exception ex)
        {
            Debug.LogError("Erreur lors du chargement des unités : " + ex.Message);
            return new List<Unit>();
        }
    }
}