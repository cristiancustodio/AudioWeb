using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Net.Http.Json;
using System.Text.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

// Carrega o appsettings.json manualmente da wwwroot
using var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var root = await http.GetFromJsonAsync<JsonElement>("appsettings.json");
if (!root.TryGetProperty("Supabase", out var supabaseSection))
    throw new InvalidOperationException("Seção 'Supabase' não encontrada em wwwroot/appsettings.json.");
var config = supabaseSection.Deserialize<AudioWeb.Client.Configuration.SupabaseOptions>();
if (config == null)
    throw new InvalidOperationException("Configuração 'Supabase' inválida em wwwroot/appsettings.json.");
builder.Services.AddScoped(sp => new AudioWeb.Client.Services.SupabaseService(config));

await builder.Build().RunAsync();
