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

    public static async Task<Book> GetBookById(Guid bookId)
    {
        try
        {
            var result = await Supabase.Client.Instance
                .From<Book>()
                .Filter("id", Postgrest.Constants.Operator.Equals, bookId.ToString())
                .Get();

            if (result.Models.Count > 0)
                return result.Models[0];

            return null;
        }
        catch (Exception ex)
        {
            Debug.LogError("Erreur lors du chargement du livre : " + ex.Message);
            return null;
        }
    }
}