namespace AudioWeb.Client.Configuration
{
    public class SupabaseOptions
    {
        public const string SectionName = "Supabase";
        
        public string Url { get; set; } = "https://xfsuqfyuwkwssmhtrify.supabase.co";
        public string Anon_Key { get; set; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Inhmc3VxZnl1d2t3c3NtaHRyaWZ5Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTA0NDkxODUsImV4cCI6MjA2NjAyNTE4NX0.eQzwkLQkdAcf0qL7D2ObzdVXT2ImmjIc3G37rnOCV5A";
        public string ProjectId { get; set; } = "xfsuqfyuwkwssmhtrify";
    }
}
