using Microsoft.EntityFrameworkCore;
using TaskBoard.API.Extension;
using TaskBoard.DAL;
using TaskBoard.DAL.Mappings;
using TaskBoard.Infrastructure.Authentication;
using static CSharpFunctionalExtensions.Result;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddRazorPages();
services.AddDbContext<TaskBoardDbContext>(
    options =>
    {
        options.UseSqlServer(configuration.GetConnectionString(nameof(TaskBoardDbContext)));
    });

services.AddAutoMapper(typeof(DataBaseMappings));


services.AddApiAuthentication(configuration);

services.AddControllersWithViews();

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

services.InitializeRepositories();
services.InitializeServices();

services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();

app.Use(async (context, next) =>
{
    if (!context.User.Identity.IsAuthenticated && !context.Request.Path.StartsWithSegments("/Account"))
    {
        context.Response.Redirect("/Account/Login");
        return;
    }

    await next();
});

app.UseAuthorization();




app.MapControllerRoute(
        name: "login",
        pattern: "Account/Login",
        defaults: new { controller = "Account", action = "Login" });

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Projects}/{action=GetAll}/{id?}");


app.Run();