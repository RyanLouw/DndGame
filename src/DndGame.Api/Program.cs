using DndGame.Data;               // namespace from scaffolded project
using DndGame.Data.Entities;
using DndGame.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<DndGameContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DND Game API",
        Version = "v1"
    });
});

// CORS (allow local dev UI later)
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p => p
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowed(_ => true) // dev only
    );
});

// Controllers and services
builder.Services.AddControllers();
builder.Services.AddScoped<IPingDataAccess, PingDataAccess>();
builder.Services.AddScoped<PingManager>();

var app = builder.Build();
app.UseCors();

// Always expose the OpenAPI spec; only enable UI in development
app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "DND Game API v1"));
}

// Health
app.MapGet("/", () => "DND Game API OK");

// Controllers
app.MapControllers();



app.Run();
