//<<<<<<< HEAD
﻿//using Microsoft.EntityFrameworkCore;
//using YourProjectNamespace.Data;

//var builder = WebApplication.CreateBuilder(args);  // 🔹 спочатку створюємо builder

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//// 🔹 Підключення DbContext до PostgreSQL
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}
//=======


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
app.UseRouting();
app.UseAuthorization();

//>>>>>>> 231efffe486855baa9f2ab2c17ba3ad3ffc6a08d

//app.UseHttpsRedirection();
//app.UseStaticFiles();  // якщо є статичні файли

//app.UseRouting();

// HEAD
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();
//=======
app.Lifetime.ApplicationStarted.Register(() =>
{
    var url = "https://localhost:7072/api/ai/run-test";

    try
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
    catch { }
});

app.Run();
//>>>>>>> 231efffe486855baa9f2ab2c17ba3ad3ffc6a08d
