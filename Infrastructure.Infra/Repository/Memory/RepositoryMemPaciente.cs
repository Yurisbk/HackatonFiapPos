namespace Infrastructure.Infra.Repository.Memory;

using Domain.Entity;
using Domain.Interfaces.Repository;
using Force.DeepCloner;

public class RepositoryMemPaciente : IRepositorioPaciente
{
    List<Paciente> Pacientes = new();

    public Paciente? ResgatarPacientePorId(int id) => Pacientes.FirstOrDefault(p => p.Id == id)?.DeepClone();

    public Paciente? ResgatarPacientePorEmail(string email) => Pacientes.FirstOrDefault(p => p.EMail == email)?.DeepClone();

    public void RegistarNovoPaciente(Paciente paciente)
    {
        ArgumentNullException.ThrowIfNull(paciente);

        paciente.Validar();

        // Simula UKs
        if (Pacientes.Any(p => p.CPF == paciente.CPF))
            throw new ArgumentException("CPF já cadastrado");

        if (Pacientes.Any(p => p.EMail == paciente.EMail))
            throw new ArgumentException("Email já cadastrado");

        // Simula PK
        paciente.Id = Pacientes.Count == 0 ? 0 : Pacientes.Max(p => p.Id) + 1;

        Pacientes.Add(paciente.DeepClone());
    }

    public void AlterarDadosPaciente(Paciente paciente)
    {
        ArgumentNullException.ThrowIfNull(paciente);

        paciente.Validar();

        Paciente? pacienteCadastro = ResgatarPacientePorId(paciente.Id ?? -1);
        if (pacienteCadastro == null)
            throw new ArgumentException("Paciente não encontrado");

        // Simula UKs
        if (Pacientes.FirstOrDefault(p => p.CPF == paciente.CPF) != pacienteCadastro)
            throw new ArgumentException("CPF já cadastrado");

        if (Pacientes.FirstOrDefault(p => p.EMail == paciente.EMail) != pacienteCadastro)
            throw new ArgumentException("Email já cadastrado");

        Pacientes[Pacientes.IndexOf(pacienteCadastro)] = paciente.DeepClone();
    }

    public void ExcluirPaciente(int id)
    {
        Paciente? pacienteCadastro = ResgatarPacientePorId(id);
        if (pacienteCadastro == null)
            throw new ArgumentException("Paciente não encontrado");

        Pacientes.RemoveAt(Pacientes.FirstOrDefault(p => p.Id == id)?.Id ?? -1);
    }
}
