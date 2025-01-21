using System.Configuration;
using System.Text;
using System.Text.Json;
using DAL;
using GameBrain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApp.Hubs;

// Set up App Config
var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.WebHost.UseStaticWebAssets();

//builder.Services
//.AddTransient<>(); - create new one every time
//.AddSingleton<>(); - create new one on first try, all the next requests get existing
//.AddScoped<>(); - create new one for every web request

builder.Services.AddScoped<ConfigRepository>();
builder.Services.AddScoped<SessionRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<JwtTokenHelper>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorPages(o =>
{
    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase
);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(365);
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// To available downloads from site
app.UseStaticFiles();

//Using Session
app.UseSession();

//Using jwt token
app.UseAuthentication();
app.UseMiddleware<JwtMiddleware>(); //using jwt middleware

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseExceptionHandler("/Error");

app.MapHub<GameHub>("/gameHub");

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();