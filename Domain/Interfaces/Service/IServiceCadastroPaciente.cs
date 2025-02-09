using Domain.DTO;
using Domain.Entity;

namespace Domain.Interfaces.Service;

public interface IServiceCadastroPaciente
{
    Task ExcluirPaciente(int id);
    Task GravarPaciente(Paciente paciente);
    Task<Paciente?> ResgatarPacientePorEmail(string email);
    Task<Paciente?> ResgatarPacientePorCpf(string cpf);
    Task<DTOAutenticacaoResponse?> LoginPaciente(DTOLoginPaciente loginRequest);
    Task<DTOCreateUsuarioResponse?> CriarPaciente(DTOCreatePaciente createPaciente);
}
