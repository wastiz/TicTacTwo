using System.Text;
using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApp.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Добавляем поддержку БД (например, PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавление jwt и identity
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = builder.Configuration["Jwt:Issuer"],
      ValidAudience = builder.Configuration["Jwt:Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
  });

builder.Services.AddAuthorization(); 

// SignalR для реального времени
builder.Services.AddSignalR();

// Настройка CORS (чтобы клиент мог подключаться)
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowBlazorClient", policy =>
  {
    policy.WithOrigins("https://localhost:5001") // URL Blazor WASM
      .AllowAnyHeader()
      .AllowAnyMethod();
  });
});

var app = builder.Build();

app.UseCors("AllowBlazorClient");
app.MapHub<GameHub>("/gameHub"); // SignalR
app.MapControllers();
app.Run();
