using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class BookService
{
    public static async Task<List<Book>> GetAllBooks()
    {
        try
        {
            var result = await Supabase.Client.Instance
                .From<Book>()
                .Get();

            return result.Models;
        }
        catch (Exception ex)
        {
            Debug.LogError("Erreur lors du chargement des livres : " + ex.Message);
            return new List<Book>();
        }
    }
}