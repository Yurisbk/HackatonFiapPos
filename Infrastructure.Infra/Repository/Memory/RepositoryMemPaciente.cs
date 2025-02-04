namespace Infrastructure.Repository.Memory;

using Domain.Entity;
using Domain.Interfaces.Repository;
using Force.DeepCloner;

public class RepositoryMemPaciente : IRepositoryPaciente
{
    public async Task<Paciente?> ResgatarPacientePorId(int id) => await Task.FromResult(MemDB.Pacientes.FirstOrDefault(p => p.Id == id)?.DeepClone());

    public async Task<Paciente?> ResgatarPacientePorEmail(string email) => await Task.FromResult(MemDB.Pacientes.FirstOrDefault(p => p.EMail == email)?.DeepClone());

    public async Task RegistarNovoPaciente(Paciente paciente)
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

        await Task.CompletedTask;
    }

    public async Task AlterarDadosPaciente(Paciente paciente)
    {
        ArgumentNullException.ThrowIfNull(paciente);

        paciente.Validar();

        Paciente? pacienteCadastro = await ResgatarPacientePorId(paciente.Id ?? -1);
        if (pacienteCadastro == null)
            throw new ArgumentException("Paciente não encontrado");

        // Simula UKs
        if (MemDB.Pacientes.FirstOrDefault(p => p.CPF == paciente.CPF) != pacienteCadastro)
            throw new ArgumentException("CPF já cadastrado");

        if (MemDB.Pacientes.FirstOrDefault(p => p.EMail == paciente.EMail) != pacienteCadastro)
            throw new ArgumentException("Email já cadastrado");

        MemDB.Pacientes[MemDB.Pacientes.IndexOf(pacienteCadastro)] = paciente.DeepClone();
    }

    public async Task ExcluirPaciente(int id)
    {
        Paciente? pacienteCadastro = await ResgatarPacientePorId(id);
        if (pacienteCadastro == null)
            throw new ArgumentException("Paciente não encontrado");

        MemDB.Pacientes.RemoveAt(MemDB.Pacientes.FirstOrDefault(p => p.Id == id)?.Id ?? -1);
    }
}
