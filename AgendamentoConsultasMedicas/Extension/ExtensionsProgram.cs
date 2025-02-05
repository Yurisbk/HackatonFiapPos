using Domain.DTO;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Infrastructure.DataAutenticador;
using Infrastructure.Repository.Memory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        services.AddScoped<IServiceCadastroPaciente, ServiceCadastroPaciente>();
        services.AddScoped<IRepositoryPaciente, RepositoryMemPaciente>();
        return services;
    }
    public static IServiceCollection AddConfiguracaoAPIAuthenticacao(this IServiceCollection services, IConfiguration configuration)
    {

        var apiAutenticacao = configuration.GetValue<string>("APIAutenticacao");
        services.AddHttpClient("AutenticacaoAPI", client =>
        {
            client.BaseAddress = new Uri(apiAutenticacao);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        return services;
    }
    public static IServiceCollection AddDocumentacaoSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Health&Med API" });

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
        });

        return services;
    }

    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AutenticacaoDbContext>(options =>
             options.UseNpgsql(configuration.GetConnectionString("APIAutenticacao")));

        return services;
    }
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var key = configuration["Jwt:Secret"];

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:7168/";
                options.Audience = configuration["Jwt:Audience"];
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Secret"])
                    )
                };
            });

        return services;
    }
}
