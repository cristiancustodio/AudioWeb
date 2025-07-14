using MudBlazor;
using AudioWeb.Shared.Models;

namespace AudioWeb.Client.Pages;

public partial class Audiometros
{
    public string GetDateStatus(DateTime date)
    {
        var daysDiff = (DateTime.Today - date).Days;
        return daysDiff switch
        {
            < 30 => "Recente",
            < 90 => $"Há {daysDiff} dias",
            < 365 => $"Há {daysDiff / 30} meses",
            _ => "Desatualizado"
        };
    }

    public Color GetDateStatusColor(DateTime date)
    {
        var daysDiff = (DateTime.Today - date).Days;
        return daysDiff switch
        {
            < 90 => Color.Success,
            < 365 => Color.Warning,
            _ => Color.Error
        };
    }

    private string GetOverallStatus(AudiometroModel audiometro)
    {
        if (!audiometro.DataCalibracao.HasValue && !audiometro.DataAfericao.HasValue)
            return "Pendente Configuração";

#pragma warning disable CS8629 // Nullable value type may be null.
        var oldestDate = new[] { audiometro.DataCalibracao, audiometro.DataAfericao }
            .Where(d => d.HasValue)
            .Select(d => d.Value!)
            .DefaultIfEmpty(DateTime.MinValue)
            .Min();
#pragma warning restore CS8629 // Nullable value type may be null.

        if (oldestDate == DateTime.MinValue) return "Pendente";

        var daysDiff = (DateTime.Today - oldestDate).Days;
        return daysDiff switch
        {
            < 90 => "Em Dia",
            < 365 => "Atenção",
            _ => "Desatualizado"
        };
    }

    private Color GetOverallStatusColor(AudiometroModel audiometro)
    {
        var status = GetOverallStatus(audiometro);
        return status switch
        {
            "Em Dia" => Color.Success,
            "Atenção" => Color.Warning,
            "Desatualizado" => Color.Error,
            _ => Color.Secondary
        };
    }
}