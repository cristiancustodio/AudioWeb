using Supabase;
using AudioWeb.Client.Models;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace AudioWeb.Client.Services
{
    public partial class SupabaseService
    {
        private readonly Supabase.Client _client;

        public SupabaseService(Configuration.SupabaseOptions options)
        {
            var url = options.Url;
            var key = options.Anon_Key;

            Console.WriteLine($"[SupabaseService] URL carregada: '{url}'");
            Console.WriteLine($"[SupabaseService] Anon_Key está {(string.IsNullOrWhiteSpace(key) ? "vazio" : "preenchido")}");
            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException($"Configuração Supabase inválida. Url='{url}', Anon_Key preenchido: {!string.IsNullOrWhiteSpace(key)}");

            var supabaseOptions = new Supabase.SupabaseOptions
            {
                AutoRefreshToken = false, // Corrigido para WebAssembly
                AutoConnectRealtime = false // Corrigido para WebAssembly
            };

            _client = new Supabase.Client(url, key, supabaseOptions);
        }

        public Supabase.Client GetClient() => _client;

        // Método para executar consulta SQL customizada
        public async Task<string> ExecuteRawQuery(string query)
        {
            try
            {
                await _client.InitializeAsync();
                var result = await _client.Rpc(query, new { });
                return result?.Content ?? "Nenhum resultado encontrado";
            }
            catch (Exception ex)
            {
                return $"Erro ao executar consulta: {ex.Message}";
            }
        }
        
        public async Task<DateTime?> GetCurrentDate()
        {
            try
            {
                await _client.InitializeAsync();
                var result = await _client.Rpc("get_current_date", new {});
                
                if (result?.Content != null)
                {
                    if (DateTime.TryParse(result.Content, out DateTime currentDate))
                    {
                        return currentDate;
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter data atual: {ex.Message}");
                return null;
            }
        }
    

    }

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

    public partial class SupabaseService
    {
        public async Task<List<AudiometerModel>> GetAudiometrosAsync()
        {
            Console.WriteLine($"[SupabaseService] GetAudiometrosAsync - URL: '{_client.ToString()}'");
            await _client.InitializeAsync();
            var result = await _client.From<AudiometroEntity>().Get();
            return result.Models.Select(e => new AudiometerModel
            {
                Id = e.Id,
                Description = e.Description,
                CalibrationDate = e.CalibrationDate,
                MeasurementDate = e.MeasurementDate
            }).ToList();
        }

        public async Task<AudiometerModel?> InsertAudiometroAsync(AudiometerModel model)
        {
            Console.WriteLine($"[SupabaseService] InsertAudiometroAsync - URL: '{_client.ToString()}'");
            await _client.InitializeAsync();
            var entity = new AudiometroEntity
            {
                Description = model.Description,
                CalibrationDate = model.CalibrationDate,
                MeasurementDate = model.MeasurementDate
            };
            var inserted = await _client.From<AudiometroEntity>().Insert(entity);
            var first = inserted.Models.FirstOrDefault();
            if (first == null) return null;
            return new AudiometerModel
            {
                Id = first.Id,
                Description = first.Description,
                CalibrationDate = first.CalibrationDate,
                MeasurementDate = first.MeasurementDate
            };
        }

        public async Task<bool> UpdateAudiometroAsync(AudiometerModel model)
        {
            Console.WriteLine($"[SupabaseService] UpdateAudiometroAsync - URL: '{_client.ToString()}'");
            await _client.InitializeAsync();
            var entity = new AudiometroEntity
            {
                Id = model.Id,
                Description = model.Description,
                CalibrationDate = model.CalibrationDate,
                MeasurementDate = model.MeasurementDate
            };
            var updated = await _client.From<AudiometroEntity>().Update(entity);
            return updated.Models.Any();
        }

        public async Task<bool> DeleteAudiometroAsync(int id)
        {
            Console.WriteLine($"[SupabaseService] DeleteAudiometroAsync - URL: '{_client.ToString()}'");
            await _client.InitializeAsync();
            var entity = new AudiometroEntity { Id = id };
            var deleted = await _client.From<AudiometroEntity>().Delete(entity);
            return deleted.Models.Any();
        }
    }
}
