using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using SimplePlanningPoker;
using SimplePlanningPoker.Hubs;
using SimplePlanningPoker.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();
builder.Services.AddSignalR();

builder.Services.AddSingleton<IRoomManager, RoomManager>();
builder.Services.AddSingleton<IUserManager, UserManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Development Environment
    app.UseSwaggerUi3();
    app.UseOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapHub<RoomHub>("/roomhub").AllowAnonymous();

app.MapFallbackToFile("index.html");

app.Run();

