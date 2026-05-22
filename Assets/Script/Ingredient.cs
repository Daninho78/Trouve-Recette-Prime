using System;
using Postgrest.Attributes;
using Postgrest.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

[Table("ingredients")]
public class Ingredient : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("alim_code")]
    public string AlimCode { get; set; }

    [Column("normalized_name")]
    public string NormalizedName { get; set; }

    
}