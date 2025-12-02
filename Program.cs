// JWT removido para MVP simples sem autenticação
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SIGE.API.Data;
using SIGE.API.Interfaces;
using SIGE.API.Mappings;
using SIGE.API.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Entity Framework - SQLite file in Data folder
var dbRelative = builder.Configuration.GetConnectionString("DefaultConnection");
var dataFolder = Path.Combine(builder.Environment.ContentRootPath, "Data");
Directory.CreateDirectory(dataFolder);
// Normalize connection string if relative path used
string dbPath = Path.Combine(builder.Environment.ContentRootPath, dbRelative!
    .Replace("Data Source=", string.Empty)
    .Replace("\\", Path.DirectorySeparatorChar.ToString())
    .Replace("/", Path.DirectorySeparatorChar.ToString()));
var normalizedConn = $"Data Source={dbPath}";

builder.Services.AddDbContext<SIGEDbContext>(options =>
    options.UseSqlite(normalizedConn));

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<IProfessorService, ProfessorService>();
builder.Services.AddScoped<ITurmaService, TurmaService>();
builder.Services.AddScoped<ITurmaService, TurmaService>();

// Removido JWT para MVP: endpoints públicos

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

        if (allowedOrigins.Length == 0 || allowedOrigins.Contains("*"))
        {
            // Sem credenciais quando origem é aberta
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
    });
});

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// API Explorer
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SIGE API",
        Version = "v1",
        Description = "API do Sistema Integrado de Gestão Escolar",
        Contact = new OpenApiContact
        {
            Name = "SIGE Team",
            Email = "admin@sige.edu.br"
        }
    });

    // Segurança JWT omitida no MVP

    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// BCrypt
builder.Services.AddSingleton<BCrypt.Net.BCrypt>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Swagger sempre habilitado (pode alterar via configuração futura)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SIGE API V1");
    // Não ocupar raiz diretamente; expor root com redirect manual
    c.RoutePrefix = "swagger";
});

// Create database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SIGEDbContext>();
    try
    {
        // Prefer migrations (Database.Migrate) if they exist; fallback to EnsureCreated for first run
        try
        {
            context.Database.Migrate();
        }
        catch
        {
            context.Database.EnsureCreated();
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// HTTPS redirection desativado em ambiente local sem configuração de porta HTTPS
// app.UseHttpsRedirection();

// Static files for uploads
app.UseStaticFiles();

// CORS
app.UseCors("AllowAngularApp");

// Autenticação/Autorização removidas no MVP

// Global error handling
app.UseMiddleware<ErrorHandlingMiddleware>();

// Endpoints básicos
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapGet("/health", () => Results.Json(new { status = "OK", time = DateTime.UtcNow }));

app.MapControllers();

app.Run();

// Global Error Handling Middleware
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = new
        {
            error = new
            {
                message = exception.Message,
                statusCode = context.Response.StatusCode
            }
        };

        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
}