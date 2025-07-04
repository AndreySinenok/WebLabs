﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Sinenok.UI.Data;
using Sinenok.UI.Services;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("SqLiteConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddHttpClient<IProductService, ApiProductService>(opt =>
    opt.BaseAddress = new Uri("https://localhost:7002/api/gadgets/"));

builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
    opt.BaseAddress = new Uri("https://localhost:7002/api/categories/"));


builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p =>
    p.RequireClaim(ClaimTypes.Role, "admin"));
});
builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// ����� ������ SeedData ��� ���������� ��������� ������
await DbInit.SeedData(app);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();