using Domain.Entity;

namespace Domain.Interfaces.Repository;

public interface IRepositoryPaciente
{
    Paciente? ResgatarPacientePorId(int id);
    Paciente? ResgatarPacientePorEmail(string email);
    void RegistarNovoPaciente(Paciente paciente);
    void AlterarDadosPaciente(Paciente paciente);
    void ExcluirPaciente(int id);
}
