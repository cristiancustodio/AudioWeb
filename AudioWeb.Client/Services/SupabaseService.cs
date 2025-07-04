using Supabase;
using AudioWeb.Client.Configuration;

namespace AudioWeb.Client.Services
{
    public class SupabaseService
    {
        private readonly Supabase.Client _client;

        public SupabaseService(AudioWeb.Client.Configuration.SupabaseOptions options)
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
    }
}
