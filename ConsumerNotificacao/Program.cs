using Domain.DTO;
using ConsumerNotificacao.Event;
using ConsumerNotificacao.Service;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace ConsumerNotificacao
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            builder.Services.AddScoped<IEmailService, EmailService>();

            var fila = configuration.GetSection("MassTransit")["NomeFila"] ?? string.Empty;
            var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
            var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
            var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;

            builder.Services.AddHostedService<Worker>();

            builder.Services.AddMassTransit(x =>
            {
                // Registra o consumidor
                x.AddConsumer<EnviaEmail>();

                // Configura o RabbitMQ
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(servidor, "/", h =>
                    {
                        h.Username(usuario);
                        h.Password(senha);
                    });

                    // Configura o endpoint para consumir mensagens da fila
                    cfg.ReceiveEndpoint(fila, e =>
                    {
                        e.ConfigureConsumer<EnviaEmail>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
            var host = builder.Build();
            host.Run();
        }
    }
}