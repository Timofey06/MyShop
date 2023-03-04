using MyShop_DataMigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MyShop_Utility;
using MyShop_DataMigrations.Repository;
using MyShop_DataMigrations.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();//����� ��� ������ � �������� �� view

builder.Services.AddSession(option =>
{
    option.Cookie.Name = "Winter2022";
    //option.IdleTimeout=TimeSpan.FromSeconds(10);
});//��� ������ � ���������


builder.Services.AddDbContext<ApplicationDbContext>(
    options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
//��� ��������� ������ ��
//builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().
    AddDefaultUI().AddDefaultTokenProviders().
    AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IRepositoryCategory, RepositoryCategory>();
builder.Services.AddScoped<IRepositoryMyModel, RepositoryMyModel>();
builder.Services.AddScoped<IRepositoryProduct, RepositoryProduct>();
builder.Services.AddScoped<IRepositoryQueryHeader, RepositoryQueryHeader>();
builder.Services.AddScoped<IRepositoryQueryDetail, RepositoryQueryDetail>();
builder.Services.AddScoped<IRepositoryApplicationUser, RepositoryApplicationUser>();
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
app.MapRazorPages(); //для определения маршрута к странице
/*app.Use((context, next) =>
{
    context.Items["name"] = "Dany";
    return next.Invoke();
});*/
app.UseSession();//������������� ������
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
