using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supabase;
using Postgrest;
using Postgrest.Responses;
using Postgrest.Models;
using Postgrest.Attributes;
using UnityEngine;


[Table("books")]
public class Book : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public static class BookService
{
    public static async Task InsertBook(string title)
    {
        var book = new Book
        {
            Title = title,
            CreatedAt = DateTime.UtcNow
        };

        var client = SupabaseManager.Instance.GetClient();
        var response = await client.From<Book>().Insert(book);

        Debug.Log("Livre ajouté : " + title);
    }

    public static async Task<List<Book>> GetAllBooks()
    {
        var client = SupabaseManager.Instance.GetClient();
        var response = await client.From<Book>().Get();
        return response.Models;
    }
    
}



