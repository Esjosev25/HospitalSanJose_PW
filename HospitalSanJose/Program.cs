using AutoMapper;
using CurrieTechnologies.Razor.SweetAlert2;
using HospitalSanJose.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<HospitalDbContext>(options =>
                    options.UseMySQL(connectionString: builder.Configuration.GetConnectionString("HospitalDB")));

builder.Services.AddDistributedMemoryCache();

var configuration = new MapperConfiguration(cfg => {
    cfg.CreateMap<User, HospitalSanJoseModel.User>().ReverseMap();
    //cfg.CreateMap<Doctor, HospitalSanJoseModel.Doctor>().ReverseMap();
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddSweetAlert2();


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
//app.Use(async (context, next) =>
//{
//    // Do work that can write to the Response.

//    string[] routes = { "/auth/login", "/auth/register", "/", "/home", "/about" };
//    if(!routes.Contains(context.Request.Path.ToString()))
//    {
//        var name = context.Session.GetString("Username");
//        var userId = context.Session.GetInt32("UserId");
//        if (name == null || userId == null)
//        {
//            context.Response.Redirect("/auth/login");
//        }
//    }
   
//    await next.Invoke();
//    // Do logging or other work that doesn't write to the Response.
//});
app.Run();
