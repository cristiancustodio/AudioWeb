using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AudioWeb.Shared.Models;

public class AudiometroModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string Descricao { get; set; } = string.Empty;
    public DateTime? DataCalibracao { get; set; }
    public DateTime? DataAfericao { get; set; }
}
