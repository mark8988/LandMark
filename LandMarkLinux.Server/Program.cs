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

// �s�W CORS �A��
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:8080") // ���\���ӷ�
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
#region JWT
//�M���w�]�M�g
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
//���UJwtHelper
builder.Services.AddSingleton<JwtHelper>();

//�ϥοﶵ�Ҧ����U
builder.Services.Configure<JwtSettingsOptions>(
    builder.Configuration.GetSection("JwtSettings"));
//�]�w�{�Ҥ覡
builder.Services
  //�ϥ�bearer token�覡�{�ҨåBtoken��jwt�榡
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          // �i�H��[Authorize]�P�_����
          RoleClaimType = "roles",
          // �w�]�|�{�ҵo��H
          ValidateIssuer = true,
          ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
          // ���{�ҨϥΪ�
          ValidateAudience = false,
          // �p�G Token ���]�t key �~�ݭn���ҡA�@�볣�u��ñ���Ӥw
          ValidateIssuerSigningKey = true,
          // ñ���ҨϥΪ�key
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
    //����api�p�����O�@
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization", //��������Atype���http�ɡA�z�Lswagger�e�����{�Үɥi�H�ٲ�Bearer�e���(�p�U��)
        Type = SecuritySchemeType.Http, //�ĥ�Bearer token
        Scheme = "Bearer", //bearer�榡�ϥ�jwt
        BearerFormat = "JWT",//�{�ҩ�bhttp request��header�W
        In = ParameterLocation.Header,//�y�z
        Description = "JWT���Ҵy�z"
    });
    //�s�@�B�~���L�o���A�L�oAuthorize�BAllowAnonymous�A�ƦܬO�S����attribute
    options.OperationFilter<AuthorizeCheckOperationFilter>();

});
#endregion

builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<LandMarkContext>(options =>
options.UseSqlServer("Server=59.125.142.83;Database=LandMark;User Id=sa;Password=au/6fu0 gj4jo4dk ru4;TrustServerCertificate=true;"), ServiceLifetime.Transient);

//���q��´�޲z
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
