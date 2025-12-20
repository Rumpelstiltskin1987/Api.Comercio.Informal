// Necesitarás agregar el using donde ubicarás tus componentes (explicado abajo)
using Api.Comercio.Informal.Components;
using Api.Entities;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVICIOS ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// [NUEVO] Agregamos los servicios de Blazor (Razor Components)
// AddInteractiveServerComponents habilita la interactividad vía SignalR (Blazor Server)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

#region Configuracion del Contexto de la Base de Datos

// Tu DbContext (SQLite)
builder.Services.AddDbContext<MySQLiteContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MySQLiteContext")));

#endregion

#region Configurar IdentityOptions para el login

// --- 1. CONFIGURACIÓN ÚNICA DE IDENTITY ---
// Usamos AddIdentity porque configura Cookies, Roles y UI de una sola vez.
builder.Services.AddIdentity<Usuario, IdentityRole<int>>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<MySQLiteContext>()
.AddApiEndpoints() // Habilita los endpoints para Android (/login)
.AddDefaultTokenProviders();

// --- 2. CONFIGURACIÓN DE AUTENTICACIÓN HÍBRIDA (WEB + ANDROID) ---
builder.Services.AddAuthentication(options =>
{
    // Esquema por defecto para la Web
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddBearerToken(IdentityConstants.BearerScheme); // Esquema para Android

// --- 3. CONFIGURACIÓN DE TIEMPOS (330 MINUTOS) ---

// Para la aplicación WEB (Cookies)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "SISCOIN_Auth";
    options.LoginPath = "/login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // 330 min exactos
    options.SlidingExpiration = true;
});

// Para la aplicación ANDROID (Tokens)
builder.Services.Configure<BearerTokenOptions>(IdentityConstants.BearerScheme, options => {
    options.BearerTokenExpiration = TimeSpan.FromMinutes(330);
});

#endregion

#region Injeccion de la capa de negocios  

// Tus Servicios de Negocio (Se reutilizan perfectamente en Blazor)
builder.Services.AddScoped<Api.Business.BusinessCobrador>();
builder.Services.AddScoped<Api.Business.BusinessConcepto>();
builder.Services.AddScoped<Api.Business.BusinessFolio>();
builder.Services.AddScoped<Api.Business.BusinessGremio>();
builder.Services.AddScoped<Api.Business.BusinessLider>();
builder.Services.AddScoped<Api.Business.BusinessPadron>();
builder.Services.AddScoped<Api.Business.BusinessRecaudacion>();
builder.Services.AddScoped<Api.Business.BusinessTarifa>();

#endregion

var app = builder.Build();

app.UseCors(options =>
{
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

// --- 2. MIDDLEWARE ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// [NUEVO] Habilitamos archivos estáticos (CSS, JS, imágenes). 
// Es vital para que tu WebApp tenga estilos (Bootstrap, etc.).
app.UseStaticFiles();

app.UseAuthorization();

// [NUEVO] Habilitamos Antiforgery (Seguridad contra CSRF), requerido por Blazor .NET 8
app.UseAntiforgery();

// --- 3. ENDPOINTS ---

// Mapeamos tus controladores existentes (para la App Android)
app.MapControllers();

// [NUEVO] Mapeamos el componente raíz de Blazor.
// Nota: <App> dará error hasta que crees el archivo App.razor (ver pasos siguientes)
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapIdentityApi<Usuario>(); // Crea automáticamente rutas como /login, /register, /refresh

#region Crear roles y usario administrador al iniciar la app
// --- SECCIÓN DE INICIALIZACIÓN DE SEGURIDAD (SEEDING) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<Usuario>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

        // 1. Crear los Roles si no existen
        string[] roles = { "Cobrador", "Administrador", "IT Manager" };
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<int>(roleName));
            }
        }

        // 2. Crear el Usuario Administrador Maestro
        var emailAdmin = "admin@siscoin.com";
        var adminUser = await userManager.FindByEmailAsync(emailAdmin);

        if (adminUser == null)
        {
            var user = new Usuario
            {
                UserName = emailAdmin,
                Email = emailAdmin,
                Alias = "SuperUsuario",
                EmailConfirmed = true
            };

            // Definimos una contraseña segura temporal
            var result = await userManager.CreateAsync(user, "Admin123!");

            if (result.Succeeded)
            {
                // Le asignamos el rol más alto
                await userManager.AddToRoleAsync(user, "IT Manager");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al inicializar la base de datos.");
    }
}

#endregion

app.Run();