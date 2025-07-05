using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

// var supabaseOptions = builder.Configuration
//     .GetSection(AudioWeb.Client.Configuration.SupabaseOptions.SectionName)
//     .Get<AudioWeb.Client.Configuration.SupabaseOptions>();
// builder.Services.AddScoped(sp => new AudioWeb.Client.Services.SupabaseService(supabaseOptions!));

builder.Services.AddScoped<AudioWeb.Client.Services.SupabaseService>();

await builder.Build().RunAsync();
