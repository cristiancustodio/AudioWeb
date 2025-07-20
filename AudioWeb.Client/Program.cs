using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AudioWeb.Client;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection; // ESSENCIAL para AddHttpClient e IHttpClientFactory

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// --- Configuração do HttpClient ---
// Este HttpClient básico é para chamadas NÃO autenticadas ou para recursos externos.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Configura um HttpClient nomeado ("WebAudio.ServerAPI") para comunicação com o servidor.
// Ele adiciona automaticamente o handler para incluir o token JWT nas requisições.
builder.Services.AddHttpClient("AudioWeb.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Registra um HttpClient padrão para injeção de dependência.
// Este é o HttpClient que você injetará em seus componentes para fazer chamadas SEGURAS para as APIs do Host.
// Ele utiliza o HttpClient nomeado configurado acima.
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AudioWeb.ServerAPI"));

// --- Configuração da Autenticação WASM ---
// Configura a autenticação OpenID Connect (OIDC) usando IdentityServer.
builder.Services.AddApiAuthorization();

// --- Configuração do MudBlazor ---
builder.Services.AddMudServices();

await builder.Build().RunAsync();