namespace Paciente.Domain.Interfaces.Repositorio;

using Paciente.Domain.Entidades;

public interface IRepositorioPaciente
{
    Paciente? ResgatarPaciente(int id);
    Paciente? ResgatarPacientePorCPF(string cpf);
    Paciente? ResgatarPacientePorEmail(string email);
    void RegistarNovoPaciente(Paciente paciente);
    void AlterarDadosPaciente(Paciente paciente);
    void ExcluirPaciente(int id);
}
