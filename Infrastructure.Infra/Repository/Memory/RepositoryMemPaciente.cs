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

        MemDB.SimulaUK(MemDB.Pacientes, (a, b) => a.CPF == b.CPF, paciente, "CPF já cadastrado");
        MemDB.SimulaUK(MemDB.Pacientes, (a, b) => a.EMail == b.EMail, paciente, "Email já cadastrado");

        // Simula PK
        paciente.Id = MemDB.CriaChaveUnica(MemDB.Pacientes);

        MemDB.Pacientes.Add(paciente.DeepClone());

        await Task.CompletedTask;
    }

    public async Task AlterarDadosPaciente(Paciente paciente)
    {
        ArgumentNullException.ThrowIfNull(paciente);

        paciente.Validar();

        Paciente? pacienteCadastro = MemDB.Pacientes.FirstOrDefault(p => p.Id == paciente.Id);
        if (pacienteCadastro == null)
            throw new ArgumentException("Paciente não encontrado");

        MemDB.SimulaUK(MemDB.Pacientes, (a, b) => a.CPF == b.CPF && a.Id != b.Id, paciente, "CPF já cadastrado");
        MemDB.SimulaUK(MemDB.Pacientes, (a, b) => a.EMail == b.EMail && a.Id != b.Id, paciente, "Email já cadastrado");

        MemDB.Pacientes[MemDB.Pacientes.IndexOf(pacienteCadastro)] = paciente.DeepClone();

        await Task.CompletedTask;
    }

    public async Task ExcluirPaciente(int id)
    {
        Paciente? pacienteCadastro = await ResgatarPacientePorId(id);
        if (pacienteCadastro == null)
            throw new ArgumentException("Paciente não encontrado");

        MemDB.Pacientes.RemoveAt(MemDB.Pacientes.FirstOrDefault(p => p.Id == id)?.Id ?? -1);
    }
}
