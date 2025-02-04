using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Infrastructure.Repository.Memory;
using Microsoft.Extensions.DependencyInjection;
using Service.Service;

namespace Tests.Integration.Fixture;

public class FixtureDI : IDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public FixtureDI()
    {
        var services = new ServiceCollection();        
        
        services.AddScoped<IRepositoryPaciente, RepositoryMemPaciente>();
        services.AddScoped<IRepositoryMedico, RepositoryMemMedico>();
        services.AddScoped<IRepositoryHorarioMedico, RepositoryMemHorarioMedico>();
        services.AddScoped<IRepositoryConsulta, RepositoryMemConsulta>();

        services.AddScoped<IServiceCadastroPaciente, ServiceCadastroPaciente>();
        services.AddScoped<IServiceCadastroMedico, ServiceCadastroMedico>();
        services.AddScoped<IServiceHorarioMedico, ServiceHorarioMedico>();
        services.AddScoped<IServiceConsulta, ServiceConsulta>();

        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}

public class TestsBaseDI(FixtureDI fixtureDI) : IClassFixture<FixtureDI>
{
    protected ServiceProvider ServiceProvider => fixtureDI.ServiceProvider;
}