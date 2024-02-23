using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Verba.Abstractions.Application.Settings;
using Verba.Abstractions.FileStorage;
using Verba.Core.Application.Authentication;
using Verba.Core.Application.Authorization;
using Verba.Core.Application.JwtConfiguration;
using Verba.Core.Application.Swagger;
using Verba.Core.Infrastructure.Minio;
using Verba.Identity.Core.Queries;
using Verba.Identity.Db.Data;
using Verba.Identity.Domain.Models;
using Verba.Stock.Core.BackgroundWorkers;
using Verba.Stock.Core.Extensions;
using Verba.Stock.Db.Data;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5202, listenOptions =>
    {
        serverOptions.Limits.MaxRequestBodySize = 50 * 1024 * 1024;
    });
});

// Add services to the container.

ConfigureLogs();

builder.Host.UseSerilog();

builder.Services.Configure<JwtAuthSettings>(builder.Configuration.GetSection("JwtAuth"));
builder.Services.Configure<DbConnectionSettings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<MinioSettings>(builder.Configuration.GetSection("MinioConfig"));

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder
        .SetIsOriginAllowed(x => _ = true)
        .AllowAnyHeader()
        .AllowCredentials()
        .AllowAnyMethod();
}));

builder.Services.AddJwtAuthentification(builder.Configuration);

builder.Services.AddSwagger();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddElasticSearch(builder.Configuration);

builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<StockDbContext>(opt => opt
        .UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")))
    .AddDbContext<IdentityDbContext>(opt => opt
        .UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddIdentity<User, Role>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.Tokens.PasswordResetTokenProvider = "passwordReset";
                    options.Password.RequiredLength = 8;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = false;
                })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<PasswordResetTokenProvider<User>>("passwordReset");

//builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddSingleton<IAccountQueries, AccountQueries>();
builder.Services.AddSingleton<IFileStorage, FileStorage>();
builder.Services.AddSingleton<IDocxFormatierWorker, DocxFormatierWorker>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Permissions.Any, p => p.RequireAuthenticatedUser());
    options.AddPolicy(Permissions.Admin, p => p.RequireRole(Permissions.Admin));
    options.AddPolicy(Permissions.User, p => p.RequireRole(Permissions.User, Permissions.Admin));
    options.AddPolicy(Permissions.Uploader, p => p.RequireRole(Permissions.Uploader, Permissions.Admin));
});


//builder.Services.AddPermissionsAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseMiddleware<JwtAuthInHeaderMiddleware>();
app.UseAuthentication();
app.AllowBothJwtAndApiKeyAuthentification();
app.UseAuthorization();

app.UseApiSwagger();

if (app.Environment.IsDevelopment())
    app.UseCors("AllowAll");
else
    app.UseCors("corsapp");

app.MapControllers();

app.Run();


#region ElasticLogs

void ConfigureLogs()
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureELS(configuration, env))
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureELS(IConfigurationRoot configuration, string env)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ELKConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "LogStash"
    };
}

#endregion