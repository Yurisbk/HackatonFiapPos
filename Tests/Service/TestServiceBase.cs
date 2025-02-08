using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Infrastructure.Repository.Memory;
using Service.Service;

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

    public TestServiceBase()
    {
        RepositoryPaciente = new RepositoryMemPaciente();
        RepositoryMedico = new RepositoryMemMedico();
        RepositoryConsulta = new RepositoryMemConsulta();
        RepositoryHorarioMedico = new RepositoryMemHorarioMedico();

        TransacaoFactory = new FakeTransacaoFactory();

        ServiceHorarioMedico = new ServiceHorarioMedico(RepositoryHorarioMedico, RepositoryMedico, TransacaoFactory);
        ServiceConsulta = new ServiceConsulta(RepositoryConsulta, RepositoryMedico, ServiceHorarioMedico, TransacaoFactory);
        ServiceCadastroMedico = new ServiceCadastroMedico(RepositoryMedico, ServiceConsulta, TransacaoFactory);
        ServiceCadastroPaciente = new ServiceCadastroPaciente(RepositoryPaciente, ServiceConsulta, TransacaoFactory);
    }
}
