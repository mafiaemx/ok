using Microsoft.EntityFrameworkCore;
using ok.Ai;
using ok.Ai.Tools;
using ok.Models;
using ok.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHttpClient<LlmService>();

builder.Services.AddScoped<Rk4Service>();
builder.Services.AddScoped<BwmService>();
builder.Services.AddScoped<LogisticsService>();
builder.Services.AddScoped<LogisticsTool>();
builder.Services.AddScoped<AiOrchestrator>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
