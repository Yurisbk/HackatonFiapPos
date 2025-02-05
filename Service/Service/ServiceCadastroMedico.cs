using Domain.DTO;
using Domain.DTO.Autenticacao;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;

namespace Service.Service;

public class ServiceCadastroMedico: IServiceCadastroMedico
{
    private readonly IServiceCadastroUsuario _serviceCadastroUsuario;
    private readonly IRepositoryMedico _repositorioMedico;

    public ServiceCadastroMedico(IServiceCadastroUsuario serviceCadastroUsuario, IRepositoryMedico repositorioMedico)
    {
        _serviceCadastroUsuario = serviceCadastroUsuario;
        _repositorioMedico = repositorioMedico;
    }
    public Medico? ResgatarMedicoPorEmail(string email) => _repositorioMedico.ResgatarMedicoPorEmail(email);

    public void GravarMedico(Medico medico)
    {
        ArgumentNullException.ThrowIfNull(medico);

        medico.Validar();

        if (medico.Id == null)
            _repositorioMedico.RegistarNovoMedico(medico);
        else
            _repositorioMedico.AlterarDadosMedico(medico);
    }

    public void ExcluirMedico(int id) => _repositorioMedico.ExcluirMedico(id);

    public Medico? ResgatarMedicoPorCRM(string crm) => _repositorioMedico.ResgatarMedicoPorCRM(crm);

    public async Task<DTOAutenticacaoResponse?> LoginMedico(DTOLoginMedico loginRequest)
    {
        Medico? medico = ResgatarMedicoPorCRM(loginRequest.CRM);

        DTOLoginUsuario loginUsuario = new DTOLoginUsuario()
        {
            Email = medico.EMail,
            Password = loginRequest.Senha,
        };

        DTOAutenticacaoResponse authResponse = await _serviceCadastroUsuario.RealizarLogin(loginUsuario);

        return authResponse;
    }

    public async Task<DTOCreateUsuarioResponse?> CriarMedico(DTOCreateMedico createMedico)
    {
        DTOCreateUsuarioResponse authResponse = await _serviceCadastroUsuario.CriarUsuario(createMedico);

        Medico medico = new Medico()
        {
            Nome = createMedico.Nome,
            CPF = createMedico.CPF,
            EMail = createMedico.Email,
            CRM = createMedico.CRM,
            AuthId = authResponse.Auth_Id
        };

        GravarMedico(medico);
        return authResponse;
    }
}
