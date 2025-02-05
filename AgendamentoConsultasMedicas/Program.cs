using Npgsql;
using System.Data;
using MassTransit;
using Domain.Interfaces.Service;
using Service.Service;
using Domain.Interfaces.Repository;
using Domain.DTO;
using ContatoAPI.Extension;

namespace AgendamentoConsultasMedicas;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
        builder.Services.AddInjecoesDependencias();
        builder.Services.AddConfiguracaoAPIAuthenticacao(configuration);
        builder.Services.AddIdentityConfiguration(configuration);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDocumentacaoSwagger();

        var connectionstring = configuration.GetValue<string>("APIAutenticacao");
        builder.Services.AddScoped<IDbConnection>((connection) => new NpgsqlConnection(connectionstring));

        //adiciona masstransit
        var fila = configuration.GetSection("MassTransit")["NomeFila"] ?? string.Empty;
        var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
        var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
        var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;


        builder.Services.AddMassTransit(x =>
        {
            x.AddRequestClient<DTONotificacao>(new Uri("queue:EnviaNotificacao"));
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(servidor, "/", h =>
                {
                    h.Username(usuario);
                    h.Password(senha);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        builder.Services.AddControllers();
        //builder.Services.AddScoped<IRepositoryNotificacao, RepositoryNotificacao>();
        builder.Services.AddScoped<IServiceNotificacao, ServiceNotificacao>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
