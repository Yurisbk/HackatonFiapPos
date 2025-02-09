using Domain.DTO;
using Domain.Entity;

namespace Domain.Interfaces.Service;

public interface IServiceCadastroMedico
{
    Task<Medico?> ResgatarMedicoPorId(int id);
    Task<Medico?> ResgatarMedicoPorCRM(string email);
    Task GravarMedico(Medico medico);
    Task ExcluirMedico(int id);
    Task<string[]> ListarEspecialidadeMedicas();
    Task<Medico[]> ListarMedicosAtivosNaEspecialidade(string especialidade);
    Task<DTOAutenticacaoResponse?> LoginMedico(DTOLoginMedico loginRequest);
    Task<DTOCreateUsuarioResponse?> CriarMedico(DTOCreateMedico createPaciente);
}
