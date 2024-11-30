using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(o =>
{
    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.UseStaticFiles();

app.Run();