using Domain.DTO.Autenticacao;
using Domain.DTO;
using Domain.Entity;
using Domain.Enum;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;

namespace Service.Service;

public class ServiceCadastroPaciente(
    IRepositoryPaciente repositorioPaciente, 
    IServiceConsulta serviceConsulta, 
    ITransacaoFactory transacaoFactory,
    IServiceCadastroUsuario serviceCadastroUsuario) : IServiceCadastroPaciente
{
    public async Task<Paciente?> ResgatarPacientePorEmail(string email) => await repositorioPaciente.ResgatarPacientePorEmail(email);
    public async Task<Paciente?> ResgatarPacientePorCpf(string cpf) => await repositorioPaciente.ResgatarPacientePorCpf(cpf);

    public async Task GravarPaciente(Paciente paciente)
    {
        using (var transacao = transacaoFactory.CriaTransacao())
        {
            ArgumentNullException.ThrowIfNull(paciente);

            paciente.Validar();

            if (paciente.Id == null)
                await repositorioPaciente.RegistarNovoPaciente(paciente);
            else
                await repositorioPaciente.AlterarDadosPaciente(paciente);

            transacao.Gravar();
        }
    }

    public async Task ExcluirPaciente(int id)
    {
        using (var transacao = transacaoFactory.CriaTransacao())
        {
            var consultasAtivas = await serviceConsulta.ListarConsultasAtivasPaciente(id);
            foreach (var consultaAtiva in consultasAtivas)
                await serviceConsulta.GravarStatusConsulta(consultaAtiva.Id!.Value, StatusConsulta.Cancelada, "Paciente desligado do sistema.");

            await repositorioPaciente.ExcluirPaciente(id);

            transacao.Gravar();
        }
    }
    public async Task<DTOAutenticacaoResponse?> LoginPaciente(DTOLoginPaciente loginRequest)
    {
        Paciente? paciente = await ResgatarPacientePorCpf(loginRequest.Cpf);

        string? email = !string.IsNullOrEmpty(loginRequest?.Cpf)
                       && paciente != null ? paciente.EMail : loginRequest?.Email;

        DTOLoginUsuario loginUsuario = new DTOLoginUsuario()
        {
            Email = email,
            Password = loginRequest.Password,
        };

        DTOAutenticacaoResponse authResponse = await serviceCadastroUsuario.RealizarLogin(loginUsuario);

        return authResponse;
    }



    public async Task<DTOCreateUsuarioResponse> CriarPaciente(DTOCreatePaciente createPaciente)
    {
        DTOCreateUsuarioResponse authResponse = await serviceCadastroUsuario.CriarUsuario(createPaciente);

        Paciente paciente = new Paciente()
        {
            Nome = createPaciente.Nome,
            CPF = createPaciente.CPF,
            EMail = createPaciente.Email,
            AuthId = authResponse.Auth_Id
        };

        await GravarPaciente(paciente);
        return authResponse;
    }
}
