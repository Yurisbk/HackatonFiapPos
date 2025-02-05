using Domain.DTO;

namespace Domain.Interfaces.Repository;

public interface IRepositoryPaciente
{
    Paciente? ResgatarPacientePorId(int id);
    Paciente? ResgatarPacientePorEmail(string email);
    Paciente? ResgatarPacientePorCpf(string cpf);
    void RegistarNovoPaciente(Paciente paciente);
    void AlterarDadosPaciente(Paciente paciente);
    void ExcluirPaciente(int id);
}
