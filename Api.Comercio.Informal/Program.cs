using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Api.Entities;
// Necesitarás agregar el using donde ubicarás tus componentes (explicado abajo)
using Api.Comercio.Informal.Components;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVICIOS ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// [NUEVO] Agregamos los servicios de Blazor (Razor Components)
// AddInteractiveServerComponents habilita la interactividad vía SignalR (Blazor Server)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Tu DbContext (SQLite)
builder.Services.AddDbContext<MySQLiteContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MySQLiteContext")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<MySQLiteContext>()
    .AddDefaultTokenProviders();

// Tus Servicios de Negocio (Se reutilizan perfectamente en Blazor)
builder.Services.AddScoped<Api.Business.BusinessCobrador>();
builder.Services.AddScoped<Api.Business.BusinessConcepto>();
builder.Services.AddScoped<Api.Business.BusinessFolio>();
builder.Services.AddScoped<Api.Business.BusinessGremio>();
builder.Services.AddScoped<Api.Business.BusinessLider>();
builder.Services.AddScoped<Api.Business.BusinessPadron>();
builder.Services.AddScoped<Api.Business.BusinessRecaudacion>();
builder.Services.AddScoped<Api.Business.BusinessTarifa>();

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

app.Run();