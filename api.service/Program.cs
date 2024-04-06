using BusinessProvider.Domain.Services;
using BusinessProvider.providers;
using BusinessProvider.Services;
using DataProvider.Data;
using DataProvider.Repositories;
using Domain.Translators;
using Domain.Translators.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

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
    });

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();