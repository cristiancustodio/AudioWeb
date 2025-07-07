
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using System;

namespace AudioWeb.Shared.Entities
{
    // Classe para mapear a tabela do Supabase
    [Table("Audiometros")]
    public class AudiometroEntity : BaseModel
    {
        [PrimaryKey("Id", false)]
        public int Id { get; set; }
        [Column("Descricao")]
        public string Descricao { get; set; } = string.Empty;
        [Column("DataCalibracao")]
        public DateTime? DataCalibracao { get; set; }
        [Column("DataAfericao")]
        public DateTime? DataAfericao { get; set; }
    }
}