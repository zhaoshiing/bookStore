using Amazon.Lambda.AspNetCoreServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BookStore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("LambdaUserDatabase")); // 使用内存数据库

builder.Services.AddScoped<BookStore.Services.UserService>();

builder.Services.AddControllers(); // 添加控制器

builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();