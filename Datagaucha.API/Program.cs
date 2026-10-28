/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using System.Text;
using Datagaucha.API.Middleware;
using Datagaucha.DAL.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

var jwtKey = configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT key was not found.");

var jwtIssuer = configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("JWT issuer was not found.");

var jwtAudience = configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("JWT audience was not found.");

var jwtExpirationMinutes = configuration.GetValue<int>("Jwt:ExpirationMinutes");

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<DatagauchaDbContext>(options =>
{
    options
        .UseLazyLoadingProxies()
        .UseMySql(
            connectionString,
            new MySqlServerVersion(new Version(8, 0, 45)));
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),

            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<Datagaucha.DAL.interfaces.IUnitOfWork,
    Datagaucha.DAL.EntityFramework.EFUnitOfWork>();

builder.Services.AddScoped<JwtTokenGenerator>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("ReactPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();