using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using noCarbon.API.Infrastracture.Filters;
using noCarbon.API.Infrastracture.Middlewares;
using noCarbon.Core.Caching;
using noCarbon.Core.Configurations;
using noCarbon.Data;
using noCarbon.Data.Configurations;
using noCarbon.Services.Action;
using noCarbon.Services.Categories;
using noCarbon.Services.Historics;
using noCarbon.Services.Security;
using Serilog;
using System.Net;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var serverVersion = new MySqlServerVersion(new Version(8, 0, 30));
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
logger.Information($"application start {DateTime.Now}");
var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);
builder.Services.AddSingleton(appSettings);
builder.Services.AddMemoryCache();
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
                   options.UseMySql(
                       builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion,
                       b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
builder.Services.AddControllers(
    options =>
    {
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        options.Filters.Add<ValidationFilter>();
    }).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = appSettings.HostConfig.PropertyNameCaseInsensitive;
});
builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(typeof(Program));
if (appSettings.HostConfig.EnableSwagger)
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo { Title = "noCarbon API", Version = "v1", Description = "noCarbon" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
          {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
            });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });
}
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireUppercase = true; // on production add more secured options
    options.Password.RequireDigit = true;
    options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var Key = Encoding.UTF8.GetBytes(appSettings.SecurityConfig.Key);
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // on production make it true
        ValidateAudience = false, // on production make it true
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = appSettings.SecurityConfig.Issuer,
        ValidAudience = appSettings.SecurityConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Key),
        ClockSkew = TimeSpan.Zero
    };
    o.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
            }
            return Task.CompletedTask;
        }
    };
});
//add accessor to HttpContext
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
//repositories
if (appSettings.DistributedCacheConfig.Enabled)
{
    builder.Services.AddScoped<ILocker, DistributedCacheManager>();
    builder.Services.AddScoped<IStaticCacheManager, DistributedCacheManager>();
}
else
{
    builder.Services.AddSingleton<ILocker, MemoryCacheManager>();
    builder.Services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
}
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IHistoricService, HistoricService>();
builder.Services.AddScoped<IActionsService, ActionsService>(); 
//builder.Services.Configure<ForwardedHeadersOptions>(options =>
//{
//    options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (appSettings.HostConfig.EnableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
//app.UseForwardedHeaders(new ForwardedHeadersOptions
//{
//    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
//});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
   .RequireAuthorization();
app.UseMiddleware<HttpLoggingMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.Run();