using Entities;
using Services;
using Stub;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Pokemon API",
        Version = "v1",
        Description = "API REST pour gérer des Pokémons",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Pokemon API"
        }
    });

    // Inclure les commentaires XML dans la documentation Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 1. Configuration de la base de données (StubbedContext)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=pokemon.db";

builder.Services.AddDbContext<StubbedContext>(options =>
    options.UseSqlite(connectionString));

// 2. Faire le lien pour que PokemonDbContext pointe vers StubbedContext
builder.Services.AddScoped<PokemonDbContext>(provider =>
    provider.GetRequiredService<StubbedContext>());

// 3. ENREGISTRER LE SERVICE (C'est ce qui manque ou bloque probablement)
builder.Services.AddScoped<IPokemonService, PokemonService>();

// 4. Ajouter les contrôleurs
builder.Services.AddControllers();

var app = builder.Build();

// Ensure database is created and seed data
// Ensure database is created and seed data
await using (var scope = app.Services.CreateAsyncScope())
{
    // On demande explicitement le StubbedContext au conteneur de services
    var stubContext = scope.ServiceProvider.GetRequiredService<StubbedContext>();

    // On passe le stubContext qui est bien du type attendu
    await DatabaseSeeder.SeedAsync(stubContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Swagger UI - disponible en développement et production pour faciliter les tests
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokemon API v1");
    c.RoutePrefix = "swagger"; // Accès via /swagger
    c.DisplayRequestDuration();
});

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
