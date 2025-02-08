using AgendamentoConsultasMedicas.Configuration;
using Domain.DTO;
using MassTransit;
using Npgsql;
using System.Data;

namespace AgendamentoConsultasMedicas;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Configure dependency injection
        builder.Services.DIConfigure(builder.Configuration);

        var connectionstring = configuration.GetValue<string>("ConnectionStringPostgres");
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

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
