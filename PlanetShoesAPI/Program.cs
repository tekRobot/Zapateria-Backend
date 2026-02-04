using Microsoft.EntityFrameworkCore;
using PlanetShoesAPI.Data;
using PlanetShoesAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURACIÓN DE CORS ---
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:5173",      // Vite (React moderno) en desarrollo
                "https://zapateria-planet.vercel.app/"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});
// --- FIN CONFIGURACIÓN DE CORS ---

// 1. Configuración de la Base de Datos con Resiliencia para Hamachi
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sql => {
        sql.CommandTimeout(180);
        sql.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    }));

builder.Services.AddControllers();

// 2. Configuración de Swagger Simplificada (Sin versiones)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo // "v1" se mantiene internamente como identificador técnico, pero no lo verás en las rutas
    {
        Title = "Planet Shoes API",
        Description = "Documentación técnica de los endpoints de Planet Shoes"
    });

    //var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    //if (File.Exists(xmlPath))
    //{
    //    options.IncludeXmlComments(xmlPath);
    //}
});

// 3. Registro de Servicios
builder.Services.AddScoped<IModelosService, ModelosService>();
builder.Services.AddScoped<IVendedoresService, VendedoresService>();
builder.Services.AddScoped<IPedidosService, PedidosService>();

var app = builder.Build();

// 4. Pipeline de Swagger expuesto para el Front-end
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./swagger/v1/swagger.json", "Planet Shoes API");
    c.RoutePrefix = string.Empty; // Acceso directo al entrar a la URL

    // Esta línea oculta la sección de Schemas (Models)
    c.DefaultModelsExpandDepth(-1);
});

app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins); // CORS habilitado
app.UseAuthorization();

app.MapControllers();

app.Run();