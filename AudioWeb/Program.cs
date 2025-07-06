using MudBlazor.Services;
using AudioWeb.Client.Pages;
using AudioWeb.Components;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

var supabaseOptions = builder.Configuration
    .GetSection(AudioWeb.Client.Configuration.SupabaseOptions.SectionName)
    .Get<AudioWeb.Client.Configuration.SupabaseOptions>();
if (supabaseOptions == null)
    throw new InvalidOperationException("Configuração 'Supabase' não encontrada no appsettings.json.");
builder.Services.AddScoped(sp => new AudioWeb.Client.Services.SupabaseService(supabaseOptions!));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AudioWeb.Client._Imports).Assembly);

app.Run();
