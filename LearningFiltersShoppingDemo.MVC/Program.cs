using LearningFiltersShoppingDemo.MVC.Models;
using LearningFiltersShoppingDemo.MVC.Repository;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDbConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("LearningFiltersShoppingDemoDB_ConnectionString");
    return new SqlConnection(connectionString);
});
builder.Services.AddScoped<GenericRepo<Products>>();
builder.Services.AddScoped<GenericRepo<Categories>>();
builder.Services.AddScoped<GenericRepo<BigBasketGroceries>>();
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Add("/Views/Product/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/Grocery/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/{0}.cshtml");
    });

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
