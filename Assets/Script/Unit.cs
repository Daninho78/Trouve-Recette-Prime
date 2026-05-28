using System;
using Postgrest.Attributes;
using Postgrest.Models;

[Table("units")]
public class Unit : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("normalized_name")]
    public string NormalizedName { get; set; }

    [Column("search_priority")]
    public int SearchPriority { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
