namespace Infrastructure.Repository.Memory;

using Domain.DTO;
using Domain.Interfaces.Repository;
using Force.DeepCloner;

public class RepositoryMemPaciente : IRepositoryPaciente
{
    public Paciente? ResgatarPacientePorId(int id) => MemDB.Pacientes.FirstOrDefault(p => p.Id == id)?.DeepClone();

    public Paciente? ResgatarPacientePorEmail(string email) => MemDB.Pacientes.FirstOrDefault(p => p.EMail == email)?.DeepClone();

    public void RegistarNovoPaciente(Paciente paciente)
    {
        ArgumentNullException.ThrowIfNull(paciente);

        paciente.Validar();

        // Simula UKs
        if (MemDB.Pacientes.Any(p => p.CPF == paciente.CPF))
            throw new ArgumentException("CPF já cadastrado");

        if (MemDB.Pacientes.Any(p => p.EMail == paciente.EMail))
            throw new ArgumentException("Email já cadastrado");

        // Simula PK
        paciente.Id = MemDB.CriaChaveUnica(MemDB.Pacientes);

        MemDB.Pacientes.Add(paciente.DeepClone());
    }

    public void AlterarDadosPaciente(Paciente paciente)
    {
        ArgumentNullException.ThrowIfNull(paciente);

        paciente.Validar();

        Paciente? pacienteCadastro = ResgatarPacientePorId(paciente.Id ?? -1);
        if (pacienteCadastro == null)
            throw new ArgumentException("Paciente não encontrado");

        // Simula UKs
        if (MemDB.Pacientes.FirstOrDefault(p => p.CPF == paciente.CPF) != pacienteCadastro)
            throw new ArgumentException("CPF já cadastrado");

        if (MemDB.Pacientes.FirstOrDefault(p => p.EMail == paciente.EMail) != pacienteCadastro)
            throw new ArgumentException("Email já cadastrado");

        MemDB.Pacientes[MemDB.Pacientes.IndexOf(pacienteCadastro)] = paciente.DeepClone();
    }

    public void ExcluirPaciente(int id)
    {
        Paciente? pacienteCadastro = ResgatarPacientePorId(id);
        if (pacienteCadastro == null)
            throw new ArgumentException("Paciente não encontrado");

        MemDB.Pacientes.RemoveAt(MemDB.Pacientes.FirstOrDefault(p => p.Id == id)?.Id ?? -1);
    }
}
