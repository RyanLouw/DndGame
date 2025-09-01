using DndGame.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Optional: read from appsettings.json (wwwroot) if you want it configurable
var apiSettings = new ApiSettings();
builder.Configuration.GetSection("ApiSettings").Bind(apiSettings);
builder.Services.AddSingleton(apiSettings);

// Build a dedicated HttpClient for ApiServices with an ABSOLUTE BaseAddress
builder.Services.AddScoped(sp =>
{
    var settings = sp.GetRequiredService<ApiSettings>();
    if (!settings.Apis.TryGetValue("DndApi", out var baseUrl) || string.IsNullOrWhiteSpace(baseUrl))
        throw new InvalidOperationException("Configure ApiSettings:Apis:DndApi in appsettings.json");
    return new HttpClient { BaseAddress = new Uri(baseUrl, UriKind.Absolute) };
});

builder.Services.AddScoped<ApiServices>();
builder.Services.AddScoped<FirebaseAuthService>();

await builder.Build().RunAsync();
