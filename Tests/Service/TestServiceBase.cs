using Domain.DTO;
using Domain.DTO.Autenticacao;
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

    public class FakeNotificacao: IServiceNotificacao
    {
        public async Task EnviaNotificacao(DTONotificacao notificacao)
        {
            System.Diagnostics.Trace.WriteLine(notificacao);
            await Task.CompletedTask;
        }
    }

    public class FakeServiceCadastroUsuario : IServiceCadastroUsuario
    {
        public async Task<DTOCreateUsuarioResponse> CriarUsuario(DTOCreatePessoa createPaciente)
        {
            DTOCreateUsuarioResponse createUsuario = new() { Auth_Id = Guid.NewGuid().ToString() };

            return await Task.FromResult(createUsuario);
        }

        public async Task<DTOAutenticacaoResponse> RealizarLogin(DTOLoginUsuario loginUsuario)
        {
            DTOAutenticacaoResponse createUsuario = new() { Access_Token = Guid.NewGuid().ToString() };

            return await Task.FromResult(createUsuario);
        }
    }

    IRepositoryPaciente RepositoryPaciente;
    IRepositoryMedico RepositoryMedico;
    IRepositoryConsulta RepositoryConsulta;
    IRepositoryHorarioMedico RepositoryHorarioMedico;

    IServiceNotificacao ServiceNotificao;

    protected ITransacaoFactory TransacaoFactory;

    protected IServiceCadastroUsuario ServiceCadastroUsuario { get; }
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

        ServiceNotificao = new FakeNotificacao();

        ServiceCadastroUsuario = new FakeServiceCadastroUsuario();
        ServiceHorarioMedico = new ServiceHorarioMedico(RepositoryHorarioMedico, RepositoryMedico, TransacaoFactory);
        ServiceConsulta = new ServiceConsulta(RepositoryConsulta, RepositoryMedico, RepositoryPaciente, ServiceHorarioMedico, ServiceNotificao, TransacaoFactory);
        ServiceCadastroMedico = new ServiceCadastroMedico(RepositoryMedico, ServiceConsulta, TransacaoFactory, ServiceCadastroUsuario);
        ServiceCadastroPaciente = new ServiceCadastroPaciente(RepositoryPaciente, ServiceConsulta, TransacaoFactory, ServiceCadastroUsuario);
    }
}
