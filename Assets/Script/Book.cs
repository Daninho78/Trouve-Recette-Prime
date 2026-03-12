using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supabase;
using UnityEngine;
using Postgrest;
using Postgrest.Models;
using Postgrest.Attributes;

[Table("books")]
    public class Book : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("author")]
        public string Author { get; set; }
        [Column("collection")]
        public string Collection { get; set; }
    }