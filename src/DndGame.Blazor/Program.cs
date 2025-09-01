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

// Option B: bind ApiSettings (from wwwroot/appsettings.json in WASM)
var apiSettings = new ApiSettings();
builder.Configuration.GetSection("ApiSettings").Bind(apiSettings);
builder.Services.AddSingleton(apiSettings);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ApiServices>();
builder.Services.AddScoped<FirebaseAuthService>();

try
{
    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Application failed to start: {ex}");
    throw;
}
