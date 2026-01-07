// Necesitarás agregar el using donde ubicarás tus componentes (explicado abajo)
using Api.Comercio.Informal.Components;
using Api.Comercio.Informal.Helpers;
using Api.Entities;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVICIOS ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// [NUEVO] Agregamos los servicios de Blazor (Razor Components)
// AddInteractiveServerComponents habilita la interactividad vía SignalR (Blazor Server)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options => options.DetailedErrors = true); // <--- AGREGA ESTO

#region Configuracion del Contexto de la Base de Datos

// Tu DbContext (SQLite)
builder.Services.AddDbContext<MySQLiteContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MySQLiteContext")));

#endregion

#region Configurar IdentityOptions para el login

// --- 1. CONFIGURACIÓN ÚNICA DE IDENTITY ---
// Usamos AddIdentity porque configura Cookies, Roles y UI de una sola vez.
builder.Services.AddIdentity<Usuario, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<MySQLiteContext>()
.AddApiEndpoints() // Habilita los endpoints para Android (/login)
.AddDefaultTokenProviders()
.AddErrorDescriber<SpanishIdentityErrorDescriber>();

// 2. ¡AGREGA ESTA LÍNEA OBLIGATORIA! 
// Sin esto, el componente <CascadingAuthenticationState> no funciona.
builder.Services.AddCascadingAuthenticationState();

// --- 2. CONFIGURACIÓN DE AUTENTICACIÓN HÍBRIDA (WEB + ANDROID) ---
// ... (Tu configuración de AddIdentity existente va aquí arriba) ...

// --- INICIO DEL AJUSTE: CONFIGURACIÓN JWT Y API ---

// 1. Configurar JWT para que la API entienda el Token de Android
var jwtKey = builder.Configuration["Jwt:Key"] ?? "TuClaveSecretaSuperSeguraDebeSerLarga123!";
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    // Esto permite que convivan Cookies (Blazor) y Tokens (Android)
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// 2. EVITAR QUE LA API REDIRIJA AL LOGIN (HTML) EN CASO DE ERROR
// Esto es CRÍTICO para que Android reciba errores JSON reales y no HTML
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        }
        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }
        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});

// --- FIN DEL AJUSTE ---
// --- 3. CONFIGURACIÓN DE TIEMPOS (330 MINUTOS) ---

// Para la aplicación WEB (Cookies)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "SISCOIN_Auth";
    options.LoginPath = "/login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

// Para la aplicación ANDROID (Tokens)
builder.Services.Configure<BearerTokenOptions>(IdentityConstants.BearerScheme, options =>
{
    options.BearerTokenExpiration = TimeSpan.FromMinutes(330);
});

#endregion

#region Injeccion de la capa de negocios  

// Tus Servicios de Negocio (Se reutilizan perfectamente en Blazor)
//builder.Services.AddScoped<Api.Business.BusinessCobrador>();
builder.Services.AddScoped<Api.Business.BusinessConcepto>();
builder.Services.AddScoped<Api.Business.BusinessFolio>();
builder.Services.AddScoped<Api.Business.BusinessGremio>();
builder.Services.AddScoped<Api.Business.BusinessLider>();
builder.Services.AddScoped<Api.Business.BusinessPadron>();
builder.Services.AddScoped<Api.Business.BusinessRecaudacion>();
builder.Services.AddScoped<Api.Business.BusinessTarifa>();
builder.Services.AddScoped<Api.Business.BusinessUsuario>();
builder.Services.AddScoped<Api.Business.BusinessRol>();

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

        // Crear los Roles si no existen
        string[] roles = { "Superadmin", "IT Manager", "Supervisor", "Cobrador" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<int>(role));
        }

        // Crear el Usuario Administrador Maestro
        var adminEmail = "superadmin@siscoin.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var user = new Usuario
            {
                Nombre = "SuperUsuario",
                A_paterno = "SISCOIN",
                A_materno = "ADMIN",
                UserName = "SuperUsuario",
                Email = adminEmail,
                Usuario_alta = "System",
                EsPasswordTemporal = false,
                EmailConfirmed = true
            };

            // Definimos una contraseña segura temporal
            var result = await userManager.CreateAsync(user, "Admin123!ñ");

            if (result.Succeeded)
            {
                // Le asignamos el rol más alto
                await userManager.AddToRoleAsync(user, "Superadmin");
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

#region Crear Gremio y Lider para cobradores Eventuales

// --- INICIO DE SEEDING PARA EVENTUALES ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Necesitamos el Contexto de Datos normal, no el UserManager
        var context = services.GetRequiredService<MySQLiteContext>();

        // 1. Asegurar que exista el LÍDER EVENTUAL
        // Buscamos por nombre para no duplicar
        var liderEventual = await context.Lider
            .FirstOrDefaultAsync(l => l.Nombre == "LIDER" && l.A_paterno == "EVENTUAL");

        if (liderEventual == null)
        {
            liderEventual = new Api.Entities.Lider
            {
                Nombre = "LIDER",
                A_paterno = "EVENTUAL",
                A_materno = "SISTEMA",
                Telefono = "-",
                Email = "-",
                Direccion = "-",
                Estado = "A",
                Usuario_alta = "System",
                Fecha_alta = DateTime.Now
            };

            context.Lider.Add(liderEventual);
            await context.SaveChangesAsync(); // Guardamos para generar el ID_LIDER

            var log = new Api.Entities.LiderLog
            {
                Id_movimiento = 1,
                Id_lider = liderEventual.Id_lider,
                Nombre = liderEventual.Nombre,
                A_paterno = liderEventual.A_paterno,
                A_materno = liderEventual.A_materno,
                Telefono = liderEventual.Telefono,
                Email = liderEventual.Email,
                Direccion = liderEventual.Direccion,
                Estado = liderEventual.Estado,
                Tipo_movimiento = "A",
                Usuario_modificacion = liderEventual.Usuario_alta,
                Fecha_modificacion = liderEventual.Fecha_alta
            };

            context.LiderLog.Add(log);
            await context.SaveChangesAsync();
        }

        // 2. Asegurar que exista el GREMIO EVENTUALES
        var gremioEventual = await context.Gremio
            .FirstOrDefaultAsync(g => g.Descripcion == "EVENTUALES");

        if (gremioEventual == null)
        {
            gremioEventual = new Api.Entities.Gremio
            {
                Descripcion = "EVENTUALES",
                Id_lider = liderEventual.Id_lider,
                Estado = "A",
                Usuario_alta = "System",
                Fecha_alta = DateTime.Now,
                Usuario_modificacion = "System",
                Fecha_modificacion = DateTime.Now
            };

            context.Gremio.Add(gremioEventual);
            await context.SaveChangesAsync();

            var log = new Api.Entities.GremioLog
            {
                Id_movimiento = 1,
                Id_gremio = gremioEventual.Id_gremio,
                Descripcion = gremioEventual.Descripcion,
                Id_lider = gremioEventual.Id_lider,
                Estado = gremioEventual.Estado,
                Tipo_movimiento = "A",
                Usuario_modificacion = gremioEventual.Usuario_alta,
                Fecha_modificacion = gremioEventual.Fecha_alta
            };

            context.GremioLog.Add(log);
            await context.SaveChangesAsync();
        }

        // 3. Crear el folio para el gremio EVENTUALES si no existe
        var folioEventual = await context.Folio
            .FirstOrDefaultAsync(f => f.Id_gremio == gremioEventual.Id_gremio);

        if (folioEventual == null)
        {
            folioEventual = new Api.Entities.Folio
            {
                Id_gremio = gremioEventual.Id_gremio,
                Descripcion = "Folio para cobradores eventuales",
                Prefijo = "EVT",
                Anio_vigente = DateTime.Now.Year,
                Siguiente_folio = 1
            };
            context.Folio.Add(folioEventual);
            await context.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al crear los datos semilla de Eventuales.");
    }
}
// --- FIN DE SEEDING PARA EVENTUALES ---

#endregion

#region Cierrar sesion

app.MapPost("Account/Logout", async (
    SignInManager<Usuario> signInManager,
    [FromForm] string returnUrl) =>
{
    await signInManager.SignOutAsync();
    return TypedResults.LocalRedirect(returnUrl ?? "/login");
});

#endregion

app.Run();