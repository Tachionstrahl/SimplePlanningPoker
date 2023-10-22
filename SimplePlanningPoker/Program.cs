using SimplePlanningPoker.Hubs;
using SimplePlanningPoker.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IRoomManager, RoomManager>();


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
    app.UseSwaggerUi3(c => c.DocumentTitle = "Simple Planning Poker API");
    app.UseOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapHub<RoomHub>("/Room").AllowAnonymous();

app.MapFallbackToFile("index.html");

app.Run();

