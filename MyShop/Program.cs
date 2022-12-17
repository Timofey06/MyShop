using MyShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();//нужно для работы с сессиями во view

builder.Services.AddSession(option =>
{
    option.Cookie.Name = "Winter2022";
    //option.IdleTimeout=TimeSpan.FromSeconds(10);
});//для работы с сервисами


builder.Services.AddDbContext<ApplicationDbContext>(
    options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
//для генерации таблиц бд
builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
/*app.Use((context, next) =>
{
    context.Items["name"] = "Dany";
    return next.Invoke();
});*/
app.UseSession();//использование сессии
/*app.Run(x =>
{

    //return x.Response.WriteAsync("Hello " + x.Items["name"]);
    if (x.Session.Keys.Contains("name"))
    {
       
        return x.Response.WriteAsync(x.Session.GetString("name"));
    }
    else
    {
        x.Session.SetString("name", "Uasya");
        return x.Response.WriteAsync("no");
    }
});*/
app.Run();
