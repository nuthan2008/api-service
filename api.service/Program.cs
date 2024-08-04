using System.Reflection;
using System.Security.Claims;
using APIService.Authorization;
using APIService.Extensions;
using APIService.Filters.ActionFilter;
using APIService.Handlers;
using BusinessProvider.Domain.Services;
using BusinessProvider.providers;
using BusinessProvider.Services;
using DataProvider.Data;
using DataProvider.Repositories;
using Domain.Translators;
using Domain.Translators.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Nest;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder
                .WithOrigins("http://localhost:7099")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
    });

var auth0Settings = builder.Configuration.GetSection("Auth0");
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = auth0Settings["Auth0:Domain"];
        options.Audience = auth0Settings["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

builder.Services.AddTransient<HttpRequestHeaderMiddleware>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// builder.Services.AddControllers(options =>
//     {
//         options.Filters.Add(typeof(ApiResponseWrapperFilterAttribute));
//     });
builder.Services.AddControllers();
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IBusinessLogicProvider, BusinessLogicProvider>();
builder.Services.AddAbstractFactory<IDataService, DataService>();
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IAccountTranslator, AccountTranslator>();
builder.Services.AddSingleton<IAuth0UserManager, Auth0UserManager>();


// Configure ElasticSearchSettings
var elasticsearchUri = builder.Configuration.GetSection("ElasticConfiguration:Uri").Value;
var elasticsearchDefaultIndex = builder.Configuration.GetSection("ElasticConfiguration:index").Value;
if (string.IsNullOrEmpty(elasticsearchUri))
{
    throw new ArgumentNullException("Uri", "Elasticsearch Uri cannot be null or empty.");
}

// Create and configure the ConnectionSettings
var settings = new ConnectionSettings(new Uri(elasticsearchUri))
    .DefaultIndex(elasticsearchDefaultIndex);

// Register the IElasticClient with the settings
var client = new ElasticClient(settings);
builder.Services.AddSingleton<IElasticClient>(client);
builder.Services.AddScoped(typeof(IElasticSearchService<>), typeof(ElasticSearchService<>));

ConfigureLogging();

builder.Host.UseSerilog();
builder.Services.AddSingleton(Log.Logger);
builder.Services.AddElasticSearch(builder.Configuration);
// builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(opt => { });
app.UseRouting();

// app.UseMiddleware<HttpRequestHeaderMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
//Add support to logging request with SERILOG
app.UseSerilogRequestLogging();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();


void ConfigureLogging()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{environment}.json", optional: true
    ).Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
        .Enrich.WithProperty("Environment", environment)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
    var url = "http://localhost:9200";//configuration["ElasticConfiguration:Uri"] ?? "http://localhost:9200";
    var uri = new Uri(url);
    var assemblyName = Assembly.GetExecutingAssembly().GetName().Name.IsNullOrEmpty() ?
    Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-") :
    "Random Assembly Name";
    return new ElasticsearchSinkOptions(uri)
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{assemblyName}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
        NumberOfReplicas = 1,
        NumberOfShards = 2
    };
}