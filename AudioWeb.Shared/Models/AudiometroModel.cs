    namespace AudioWeb.Shared.Models;

    public class AudiometroModel
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime? DataCalibracao { get; set; }
    public DateTime? DataAfericao { get; set; }
}
