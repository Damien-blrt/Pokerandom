using Entities;
using Services;
using Stub;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization; // AJOUT : Pour les enums

var builder = WebApplication.CreateBuilder(args);

// 1. Gestion des enums et contrôleurs
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Convertit les enums (TypePkm) en texte ("Feu") au lieu de chiffres (2)
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Pokemon API", Version = "v1" });
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath)) { c.IncludeXmlComments(xmlPath); }
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Configuration DB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=pokemon.db";
builder.Services.AddDbContext<StubbedContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<PokemonDbContext>(provider => provider.GetRequiredService<StubbedContext>());
builder.Services.AddScoped<IPokemonService, PokemonService>();

var app = builder.Build();

// Seeding au démarrage
await using (var scope = app.Services.CreateAsyncScope())
{
    var stubContext = scope.ServiceProvider.GetRequiredService<StubbedContext>();
    await DatabaseSeeder.SeedAsync(stubContext);
}

// 2. CONFIGURATION SWAGGER POUR PRODUCTION
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokemon API v1");
    c.RoutePrefix = string.Empty;
    c.DisplayRequestDuration();
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();