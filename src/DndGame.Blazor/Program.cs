using DndGame.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Bind ApiSettings from appsettings.json under wwwroot
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// HttpClient can be bare; ApiServices builds absolute URLs
builder.Services.AddScoped(sp => new HttpClient());
builder.Services.AddScoped<ApiServices>();
builder.Services.AddScoped<FirebaseAuthService>();

// (Optional sanity check during dev)
Console.WriteLine("DndApi = " + builder.Configuration["ApiSettings:Apis:DndApi"]);

await builder.Build().RunAsync();
