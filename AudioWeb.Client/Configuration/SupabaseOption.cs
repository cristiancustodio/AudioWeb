namespace AudioWeb.Client.Configuration
{
    public class SupabaseOptions
    {
        public const string SectionName = "Supabase";
        
        public string Url { get; set; } = "";
        public string Anon_Key { get; set; } = "";
        public string ProjectId { get; set; } = "";
    }
}
