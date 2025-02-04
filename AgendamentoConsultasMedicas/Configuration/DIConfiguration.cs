using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Infrastructure.Repository.DB;
using Infrastructure.Repository.Memory;
using Npgsql;
using Service.Service;
using System.Data;

namespace AgendamentoConsultasMedicas.Configuration;

public static class DIConfiguration
{
    public static void DIConfigure(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration["MemoryDatabase"] == "true")
        {
            services.AddScoped<IRepositoryPaciente, RepositoryMemPaciente>();
            services.AddScoped<IRepositoryMedico, RepositoryMemMedico>();
            services.AddScoped<IRepositoryHorarioMedico, RepositoryMemHorarioMedico>();
            services.AddScoped<IRepositoryConsulta, RepositoryMemConsulta>();
            services.AddScoped<IDbConnection>(c => null);
        }
        else
        {
            var connectionstring = configuration.GetValue<string>("ConnectionStringPostgres");
            services.AddScoped<IDbConnection>((connection) => new NpgsqlConnection(connectionstring));
        }

        services.AddScoped<ITransacao, DBTransacao>();

        services.AddScoped<IServiceCadastroPaciente, ServiceCadastroPaciente>();
        services.AddScoped<IServiceCadastroMedico, ServiceCadastroMedico>();
        services.AddScoped<IServiceHorarioMedico, ServiceHorarioMedico>();
        services.AddScoped<IServiceConsulta, ServiceConsulta>();
    }
}
