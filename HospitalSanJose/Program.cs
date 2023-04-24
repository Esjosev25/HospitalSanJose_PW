
using HospitalSanJose.Config;
using HospitalSanJose.Functions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddTransient<AuthService>();
builder.Services.AddTransient<UsersService>();
builder.Services.AddTransient<PersonalInfosService>();
builder.Services.AddTransient<RolesService>();
builder.Services.AddTransient<UserRolesService>();                 
builder.Services.AddTransient<DepartmentsService>();
builder.Services.AddTransient<DoctorsService>();
builder.Services.AddTransient<DoctorDepartmentsService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "users/{controller=Profile}/{action=Index}/{id?}");
//Middleware que chequea si el usuario ya inicio sesion
app.Use(async (context, next) =>
{
    // Do work that can write to the Response.

    string[] routes = { "/auth/login", "/auth/register", "/", "/home", "/about", "error/404", "error/401" };
    if (!routes.Contains(context.Request.Path.ToString().ToLower()))
    {
        var name = context.Session.GetString("Username");
        var userId = context.Session.GetInt32("UserId");
        var token= context.Session.GetInt32("Token");
        var roles = context.Session.GetString("Roles");
        if (name == null || userId == null|| token == null || roles == null)
        {
            context.Response.Redirect("/auth/login");
        }
    }

    await next.Invoke();
    // Do logging or other work that doesn't write to the Response.
});
app.Run();
