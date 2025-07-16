using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AudioWeb.Client;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication; // Já está aqui, ótimo!
using System.Net.Http; // Para HttpClient
using System.Net.Http.Json;
using System.Text.Json;



var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Define o componente raiz da aplicação e onde ele será renderizado
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// --- Configuração do HttpClient ---
// Este HttpClient básico é para chamadas NÃO autenticadas ou para recursos externos.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// --- Configuração da Autenticação WASM ---
// 1. Configura um HttpClient nomeado ("WebAudio.ServerAPI") para comunicação com o servidor.
//    Ele adiciona automaticamente o handler para incluir o token JWT nas requisições.
builder.Services.AddHttpClient("AudioWeb.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// 2. Registra um HttpClient padrão para injeção de dependência.
//    Este é o HttpClient que você injetará em seus componentes para fazer chamadas SEGURAS para as APIs do Host.
//    Ele utiliza o HttpClient nomeado configurado acima.
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WebAudio.ServerAPI"));

// 3. Configura a autenticação OpenID Connect (OIDC) usando MSAL (Microsoft Authentication Library).
//    É a forma padrão para Blazor WASM se comunicar com o IdentityServer no seu Host.
builder.Services.AddMsalAuthentication(options =>
{
    // Define os escopos padrão que serão solicitados ao IdentityServer.
    // "WebAudio.HostAPI" deve ser o nome da sua API configurado no IdentityServer do Host.
    options.ProviderOptions.DefaultScopes.Add("WebAudio.HostAPI"); // Certifique-se de que este nome corresponde ao que você usou no Host

    // A autoridade (Authority) é o endereço do seu servidor Identity (o Host).
    options.ProviderOptions.Authority = builder.HostEnvironment.BaseAddress;

    // O ClientId é o identificador único do seu cliente Blazor WASM no IdentityServer.
    // Ele deve corresponder ao nome que você configurou em `appsettings.json` do seu Host,
    // sob `IdentityServer -> Clients`.
    options.ProviderOptions.ClientId = "AudioWeb.Client"; // Certifique-se de que corresponde ao Host

    // URIs de redirecionamento para o fluxo de login e logout.
    // Essas rotas são tratadas pelos componentes internos do Blazor WebAssembly.
    options.ProviderOptions.RedirectUri = "/authentication/login-callback";
    options.ProviderOptions.PostLogoutRedirectUri = "/authentication/logout-callback";
});

// --- Configuração do MudBlazor ---
// Adiciona os serviços do MudBlazor, necessários para os componentes da UI.
builder.Services.AddMudServices();

// Constrói e executa a aplicação Blazor WebAssembly.
await builder.Build().RunAsync();