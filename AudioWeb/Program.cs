using MudBlazor.Services;
using AudioWeb.Components;
using AudioWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // Adicionado para Identity
using AudioWeb.Models; // Adicionado para ApplicationUser e ApplicationRole
using Microsoft.AspNetCore.Authentication; // Adicionado para Authentication
using Microsoft.AspNetCore.Identity.UI.Services; // Adicionado, se você for usar IEmailSender

var builder = WebApplication.CreateBuilder(args);

// --- 1. Adicionar Serviços de Banco de Dados e Identity ---

// Seu DbContext existente agora herdará de IdentityDbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Necessário para o Identity, para páginas de erro em desenvolvimento
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configuração do Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; // Ou false, dependendo da sua necessidade
    // Você pode adicionar mais políticas de senha aqui se quiser
    // options.Password.RequireDigit = false;
    // options.Password.RequiredLength = 6;
})
.AddRoles<ApplicationRole>() // Habilita o uso de papéis com ApplicationRole
.AddEntityFrameworkStores<AppDbContext>(); // Conecta o Identity ao seu DbContext (AppDbContext)

// --- 2. Configuração do IdentityServer para APIs do Blazor WASM ---
builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, AppDbContext>(options =>
    {
        // Certifique-se de que o nome do cliente Blazor WASM corresponde ao que está no appsettings.json
        // e que os escopos necessários estão configurados.
        // Exemplo:
        // options.IdentityResources["openid"].UserClaims.Add("NomeCompleto");
        // options.ApiResources.Single().UserClaims.Add("role"); // Garante que roles são incluídas
    });

// Adiciona os serviços de autenticação, incluindo o esquema JWT bearer
builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

// --- 3. Serviços de UI e outros serviços da sua aplicação ---

// Add MudBlazor services
builder.Services.AddMudServices();

// Seu serviço Supabase existente
var supabaseOptions = builder.Configuration
    .GetSection(AudioWeb.Client.Configuration.SupabaseOptions.SectionName)
    .Get<AudioWeb.Client.Configuration.SupabaseOptions>();
if (supabaseOptions == null)
    throw new InvalidOperationException("Configuração 'Supabase' não encontrada no appsettings.json.");
builder.Services.AddScoped(sp => new AudioWeb.Client.Services.SupabaseService(supabaseOptions!));

// Serviços para Razor Pages (necessário para o Identity UI padrão)
builder.Services.AddRazorPages();

// Adiciona os Razor Components e Blazor WebAssembly
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// Opcional: Adicione um serviço para envio de e-mail (se RequireConfirmedAccount = true)
// builder.Services.AddTransient<IEmailSender, YourEmailSenderService>();


// --- 4. Construir o Aplicativo ---
var app = builder.Build();

// --- 5. Configurar o Pipeline de Requisições HTTP (Middlewares) ---

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint(); // Para facilitar migrations em desenvolvimento
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection(); // Sempre bom ter para segurança

app.UseStaticFiles(); // Serve arquivos estáticos (CSS, JS, imagens)
app.UseAntiforgery(); // Importante para segurança de formulários (se você tiver)

app.UseBlazorFrameworkFiles(); // Serve os arquivos do Blazor WASM

// Ordem crucial para Autenticação/Autorização:
app.UseIdentityServer(); // Middleware do IdentityServer
app.UseAuthentication(); // Middleware de autenticação
app.UseAuthorization();  // Middleware de autorização

app.MapStaticAssets(); // Se você tem Assets Mapeados
app.MapRazorPages(); // Mapeia as Razor Pages (incluindo as do Identity UI)
app.MapControllers(); // Mapeia os Controllers de API (se você criar para admin ou outros)

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AudioWeb.Client._Imports).Assembly);


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

        // --- Criar Papéis se não existirem ---
        string[] roleNames = { "Administrador", "UsuarioComum" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = roleName, Descricao = $"Papel {roleName}" }); // Adicionei Descricao
                Console.WriteLine($"Papel '{roleName}' criado com sucesso."); // Feedback no console
            }
        }

        // --- Criar um Usuário Administrador Padrão se não existir ---
        var adminEmail = "admin@seuemail.com"; // Use um email real para o admin
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true, // Considere true para evitar confirmação inicial para o admin
                NomeCompleto = "Administrador do Sistema"
            };
            var result = await userManager.CreateAsync(adminUser, "SuaSenhaSegura#123"); // **MUDA ESSA SENHA EM PRODUÇÃO!**
            if (result.Succeeded)
            {
                Console.WriteLine($"Usuário '{adminEmail}' criado com sucesso.");
                await userManager.AddToRoleAsync(adminUser, "Administrador");
                Console.WriteLine($"Papel 'Administrador' atribuído a '{adminEmail}'.");
            }
            else
            {
                Console.WriteLine($"Erro ao criar usuário '{adminEmail}':");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Description}");
                }
            }
        } else {
             Console.WriteLine($"Usuário '{adminEmail}' já existe.");
             // Garante que o papel de administrador está atribuído caso o usuário já exista mas não tenha o papel
             if (!await userManager.IsInRoleAsync(adminUser, "Administrador"))
             {
                 await userManager.AddToRoleAsync(adminUser, "Administrador");
                 Console.WriteLine($"Papel 'Administrador' atribuído a '{adminEmail}' (usuário existente).");
             }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Um erro ocorreu ao semear o banco de dados com dados de usuário e papéis.");
    }
}


app.Run();






