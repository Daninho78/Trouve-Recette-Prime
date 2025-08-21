using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supabase;
using Postgrest;
using Postgrest.Models;
using Postgrest.Responses;
using Postgrest.Attributes;
using UnityEngine;
using Unity.VisualScripting;


[Table("recipes")]
public class Recipe : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("book_id")]
    public Guid BookId { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("duration")]
    public int Duration { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

