using BoardGameTools.Server.Controllers;
using BoardGameTools.Server.Fight;
using BoardGameTools.Server.Fight.Interfaces;
using BoardGameTools.Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEntityFrameworkSqlite().AddDbContext<BoardGameToolsContext>();

builder.Services.AddTransient<IRangedAttack, RangedAttack>();
builder.Services.AddTransient<IParry, Parry>();
builder.Services.AddTransient<IPhysicalAttack, PhysicalAttack>();
builder.Services.AddTransient<ITiltedCards, TiltedCards>();
builder.Services.AddTransient<ICalculateFight, CalculateFight>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
