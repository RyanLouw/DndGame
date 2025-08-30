using DndGame.Data;
using DndGame.Domain;
using DndGame.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DndGameContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DND Game API",
        Version = "v1"
    });
});


builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p => p
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowed(_ => true)
    );
});

// Controllers and services
builder.Services.AddControllers();
builder.Services.AddScoped<IPingDataAccess, PingDataAccess>();
builder.Services.AddScoped<PingManager>();

var app = builder.Build();
app.UseCors();
app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "DND Game API v1"));
}



app.MapControllers();



app.Run();
