using Domain.DTO;
using Domain.Entity;
using Domain.Interfaces.Service;
using Infrastructure.DataAutenticador;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service.Service;
using System.Reflection;
using System.Text;

namespace ContatoAPI.Extension;

public static class ExtensionsProgram
{
    public static IServiceCollection AddInjecoesDependencias(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenService, ServiceJwtToken>();
        services.AddScoped<IServiceAuthenticacao, ServiceAutenticacao>();

        return services;
    }
    public static IServiceCollection AddDocumentacaoSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthServer API" });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Digite 'Bearer {seu_token_jwt}' para autenticar",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            c.AddSecurityDefinition("Bearer", securityScheme);

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        securityScheme,
                        Array.Empty<string>()
                    }
            });
            c.IncludeXmlComments(xmlPath);

        });

        return services;
    }

    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AutenticacaoDbContext>(options =>
             options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));

        return services;
    }
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.AllowedForNewUsers = true;
        });

        services.AddIdentity<Usuario, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddEntityFrameworkStores<AutenticacaoDbContext>()
        .AddDefaultTokenProviders();

        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
    //    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    //    {
    //        return services;
    //    }
}
