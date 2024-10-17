using LandMark.EF;
using LandMark.Middleware;
using LandMark.Middleware.services;
using LandMarkLinux.Server.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var globalSettings = builder.Configuration.GetSection("GlobalSettings").Get<GlobalSettings>();
builder.Services.AddSingleton(globalSettings);
builder.Services.AddHttpContextAccessor();

// 新增 CORS 服務
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:8080") // 允許的來源
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<LandMarkContext>(options =>
options.UseSqlServer("Server=59.125.142.83;Database=LandMark;User Id=sa;Password=au/6fu0 gj4jo4dk ru4;TrustServerCertificate=true;"), ServiceLifetime.Transient);

//公司組織管理
builder.Services.AddScoped<SAdminManage, SAdminManage>();



// Helper
builder.Services.AddScoped<JwtHelper, JwtHelper>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
