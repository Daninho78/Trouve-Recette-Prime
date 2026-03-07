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

    [Column("prep_time_minutes")]
    public int? PrepTimeMinutes { get; set; }

    [Column("cook_time_minutes")]
    public int? CookTimeMinutes { get; set; }

    [Column("rest_time_minutes")]
    public int? RestTimeMinutes { get; set; }

    [Column("serving")]
    public short? Serving { get; set; }

    [Column("page")]
    public int? Page { get; set; }

    [Column("rate")]
    public short Rate { get; set; }

    [Column("difficulty")]
    public string Difficulty { get; set; }

    [Column("remarque")]
    public string Remarque { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

