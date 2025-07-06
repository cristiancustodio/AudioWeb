
namespace AudioWeb.Models
{
    public class AudiometerModel
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CalibrationDate { get; set; }
        public DateTime MeasurementDate { get; set; }
    }
}
