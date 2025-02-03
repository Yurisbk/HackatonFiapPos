using Domain.DTO;

namespace Domain.Interfaces.Service;

public interface IServiceCadastroPaciente
{
    void ExcluirPaciente(int id);
    void GravarPaciente(Paciente paciente);
    Paciente? ResgatarPacientePorEmail(string email);
}
