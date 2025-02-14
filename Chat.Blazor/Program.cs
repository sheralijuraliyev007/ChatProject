using Blazored.LocalStorage;
using Chat.Blazor;
using Chat.Blazor.Repositories.Contracts;
using Chat.Blazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7156/") });

builder.Services.AddScoped<IUserIntegration, UserIntegration>();
builder.Services.AddScoped<IChatIntegration, ChatIntegration>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider,CustomAuthHandler>();
builder.Services.AddScoped<StorageService>();


await builder.Build().RunAsync();
