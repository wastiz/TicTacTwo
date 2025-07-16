using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;

using Client;
using Client.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//Authorization Provider
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();

//Header Handler
var apiBaseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:3005";

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

builder.Services.AddScoped<AuthHeaderHandler>();
builder.Services.AddHttpClient("AuthHttpClient", client => 
    {
        client.BaseAddress = new Uri(apiBaseUrl);
    })
    .AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddScoped(sp => 
    new HttpClient { 
        BaseAddress = new Uri(apiBaseUrl) 
    });



//For local storage
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
