
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
        [Column("Description")]
        public string Description { get; set; } = string.Empty;
        [Column("CalibrationDate")]
        public DateTime? CalibrationDate { get; set; }
        [Column("MeasurementDate")]
        public DateTime? MeasurementDate { get; set; }
    }
}