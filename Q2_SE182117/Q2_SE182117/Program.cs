using Microsoft.EntityFrameworkCore;
using Q2_SE182117.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Đăng ký Database (Bắt buộc)
builder.Services.AddDbContext<PE_PRN_25FallB5_23Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

// Add services to the container.
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");


app.Run();
