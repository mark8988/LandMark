using LandMark.EF;
using LandMark.Middleware;
using LandMark.Middleware.services;
using LandMarkLinux.Server.Filters;
using LandMarkLinux.Server.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

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
#region JWT
//清除預設映射
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
//註冊JwtHelper
builder.Services.AddSingleton<JwtHelper>();

//使用選項模式註冊
builder.Services.Configure<JwtSettingsOptions>(
    builder.Configuration.GetSection("JwtSettings"));
//設定認證方式
builder.Services
  //使用bearer token方式認證並且token用jwt格式
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          // 可以讓[Authorize]判斷角色
          RoleClaimType = "roles",
          // 預設會認證發行人
          ValidateIssuer = true,
          ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
          // 不認證使用者
          ValidateAudience = false,
          // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
          ValidateIssuerSigningKey = true,
          // 簽章所使用的key
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SignKey")))
      };
  });
#endregion

#region Swagger
builder.Services.AddSwaggerGen(options =>
{
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    //說明api如何受到保護
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization", //選擇類型，type選擇http時，透過swagger畫面做認證時可以省略Bearer前綴詞(如下圖)
        Type = SecuritySchemeType.Http, //採用Bearer token
        Scheme = "Bearer", //bearer格式使用jwt
        BearerFormat = "JWT",//認證放在http request的header上
        In = ParameterLocation.Header,//描述
        Description = "JWT驗證描述"
    });
    //製作額外的過濾器，過濾Authorize、AllowAnonymous，甚至是沒有打attribute
    options.OperationFilter<AuthorizeCheckOperationFilter>();

});
#endregion

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
