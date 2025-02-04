using Domain.Entity;

namespace Domain.Interfaces.Repository;

public interface IRepositoryPaciente
{
    Task<Paciente?> ResgatarPacientePorId(int id);
    Task<Paciente?> ResgatarPacientePorEmail(string email);
    Task RegistarNovoPaciente(Paciente paciente);
    Task AlterarDadosPaciente(Paciente paciente);
    Task ExcluirPaciente(int id);
}
