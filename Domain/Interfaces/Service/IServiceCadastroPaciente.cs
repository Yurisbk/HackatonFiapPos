using Domain.Entity;

namespace Domain.Interfaces.Service;

public interface IServiceCadastroPaciente
{
    Task ExcluirPaciente(int id);
    Task GravarPaciente(Paciente paciente);
    Task<Paciente?> ResgatarPacientePorEmail(string email);
}
