using Domain.Interfaces.Repository;
using Domain.DTO;
using Domain.Interfaces.Service;
using Domain.DTO.Autenticacao;
using System.Text.Json;
using System.Text;

namespace Service.Service;

public class ServiceCadastroPaciente : IServiceCadastroPaciente
{
    private readonly IServiceCadastroUsuario _serviceCadastroUsuario;
    private readonly IRepositoryPaciente _repositorioPaciente;

    public ServiceCadastroPaciente(IServiceCadastroUsuario serviceCadastroUsuario, IRepositoryPaciente repositorioPaciente)
    {
        _serviceCadastroUsuario = serviceCadastroUsuario;
        _repositorioPaciente = repositorioPaciente;
    }

    public Paciente? ResgatarPacientePorEmail(string email) => _repositorioPaciente.ResgatarPacientePorEmail(email);
    public Paciente? ResgatarPacientePorCpf(string cpf) => _repositorioPaciente.ResgatarPacientePorCpf(cpf);

    public void GravarPaciente(Paciente paciente)
    {
        ArgumentNullException.ThrowIfNull(paciente);

        paciente.Validar();

        if (paciente.Id == null)
            _repositorioPaciente.RegistarNovoPaciente(paciente);
        else
            _repositorioPaciente.AlterarDadosPaciente(paciente);
    }
    public async Task<DTOAutenticacaoResponse?> LoginPaciente(DTOLoginPaciente loginRequest)
    {
        Paciente? paciente = ResgatarPacientePorCpf(loginRequest.Cpf);

        string? email = !string.IsNullOrEmpty(loginRequest?.Cpf)
                       && paciente != null ? paciente.EMail : loginRequest?.Email;

        DTOLoginUsuario loginUsuario = new DTOLoginUsuario()
        {
            Email = email,
            Password = loginRequest.Password,
        };

        DTOAutenticacaoResponse authResponse = await _serviceCadastroUsuario.RealizarLogin(loginUsuario);

        return authResponse;
    }

    

    public async Task<DTOCreateUsuarioResponse> CriarPaciente(DTOCreatePaciente createPaciente)
    {
        DTOCreateUsuarioResponse authResponse = await _serviceCadastroUsuario.CriarUsuario(createPaciente);

        Paciente paciente = new Paciente()
        {
            Nome = createPaciente.Nome,
            CPF = createPaciente.CPF,
            EMail = createPaciente.Email,
            AuthId = authResponse.Auth_Id
        };

        GravarPaciente(paciente);
        return authResponse;
    }

    public void ExcluirPaciente(int id) => _repositorioPaciente.ExcluirPaciente(id);
}
