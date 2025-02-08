using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Infrastructure.Repository.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Service;
using System.Net.Http;

namespace Tests.Service;

public class TestServiceBase
{
    public class FakeTransacaoFactory : ITransacaoFactory
    {
        public ITransacao CriaTransacao()
            => new TransacaoFakeMemoria();
    }

    IRepositoryPaciente RepositoryPaciente;
    IRepositoryMedico RepositoryMedico;
    IRepositoryConsulta RepositoryConsulta;
    IRepositoryHorarioMedico RepositoryHorarioMedico;

    protected ITransacaoFactory TransacaoFactory;

    protected IServiceHorarioMedico ServiceHorarioMedico { get; }
    protected IServiceConsulta ServiceConsulta { get; }
    protected IServiceCadastroMedico ServiceCadastroMedico { get; }
    protected IServiceCadastroPaciente ServiceCadastroPaciente { get; }
    protected IServiceCadastroUsuario ServiceCadastroUsuario { get; }
    protected IHttpClientFactory HttpClientFactory { get; }

    public TestServiceBase()
    {

        HttpClientFactory = CriandoHttpClient();

        RepositoryPaciente = new RepositoryMemPaciente();
        RepositoryMedico = new RepositoryMemMedico();
        RepositoryConsulta = new RepositoryMemConsulta();
        RepositoryHorarioMedico = new RepositoryMemHorarioMedico();
        TransacaoFactory = new FakeTransacaoFactory();

        ServiceCadastroUsuario = new ServiceCadastroUsuario(HttpClientFactory, RepositoryPaciente);
        ServiceHorarioMedico = new ServiceHorarioMedico(RepositoryHorarioMedico, RepositoryMedico, TransacaoFactory);
        ServiceConsulta = new ServiceConsulta(RepositoryConsulta, RepositoryMedico, ServiceHorarioMedico, TransacaoFactory);
        ServiceCadastroMedico = new ServiceCadastroMedico(RepositoryMedico, ServiceConsulta, TransacaoFactory, ServiceCadastroUsuario);
        ServiceCadastroPaciente = new ServiceCadastroPaciente(RepositoryPaciente, ServiceConsulta, TransacaoFactory, ServiceCadastroUsuario);
    }

    public IHttpClientFactory CriandoHttpClient()
    {
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "APIAutenticacao", "https://localhost:7182/" }
            })
            .Build();

        services.AddHttpClient("AutenticacaoAPI", client =>
        {
            var apiAutenticacao = configuration.GetValue<string>("APIAutenticacao");
            client.BaseAddress = new Uri(apiAutenticacao);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<IHttpClientFactory>();
    }
}
