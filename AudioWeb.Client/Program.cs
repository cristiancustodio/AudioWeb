
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

// Carrega o appsettings.json manualmente da wwwroot
using var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var config = await http.GetFromJsonAsync<AudioWeb.Client.Configuration.SupabaseOptions>("appsettings.json");
if (config == null)
    throw new InvalidOperationException("Configuração 'Supabase' não encontrada em wwwroot/appsettings.json.");
builder.Services.AddScoped(sp => new AudioWeb.Client.Services.SupabaseService(config));

await builder.Build().RunAsync();
