using DAL;
using Microsoft.EntityFrameworkCore;

// Set up App Config
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidCastException("Connection string not found");


// register "how to create a db when somebody asks for it"
builder.Services.AddDbContext<GameConfigDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddDbContext<GameStateDbContext>(options => options.UseSqlite(connectionString));

//builder.Services
//.AddTransient<>(); - create new one every time
//.AddSingleton<>(); - create new one on first try, all the next requests get existing
//.AddScoped<>(); - create new one for every web request

//builder.Services.AddScoped<IConfigRepository, ConfigRepositoryJson>();
builder.Services.AddScoped<ConfigRepositoryDb>();
builder.Services.AddScoped<GameRepositoryDb>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();