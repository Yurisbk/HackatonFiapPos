using Domain.DTO;

namespace Domain.Interfaces.Service;

public interface IServiceCadastroPaciente
{
    void ExcluirPaciente(int id);
    void GravarPaciente(Paciente paciente);
    Paciente? ResgatarPacientePorEmail(string email);
    Paciente? ResgatarPacientePorCpf(string cpf);
    Task<DTOAutenticacaoResponse?> LoginPaciente(DTOLoginPaciente loginRequest);
    Task<DTOCreateUsuarioResponse?> CriarPaciente(DTOCreatePaciente createPaciente);
}
