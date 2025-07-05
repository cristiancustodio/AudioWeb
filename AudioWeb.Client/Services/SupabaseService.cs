using Supabase;

namespace AudioWeb.Client.Services
{
    public class SupabaseService
    {
        private readonly Supabase.Client _client;

        public SupabaseService(Configuration.SupabaseOptions options)
        {
            var url = options.Url;
            var key = options.Anon_Key;

            var supabaseOptions = new Supabase.SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
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
}
