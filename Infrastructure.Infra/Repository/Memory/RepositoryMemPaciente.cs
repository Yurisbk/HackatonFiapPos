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

        MemDB.Pacientes.CheckUK(p => p.CPF == paciente.CPF, "CPF já cadastrado");
        MemDB.Pacientes.CheckUK(p => p.EMail == paciente.EMail, "Email já cadastrado");

        MemDB.Pacientes.Insert(paciente);

        await Task.CompletedTask;
    }

    public async Task AlterarDadosPaciente(Paciente paciente)
    {
        ArgumentNullException.ThrowIfNull(paciente);

        paciente.Validar();

        MemDB.Pacientes.CheckFK(paciente.Id, "Paciente não encontrado");

        MemDB.Pacientes.CheckUK(p => p.CPF == paciente.CPF && p.Id != paciente.Id, "CPF já cadastrado");
        MemDB.Pacientes.CheckUK(p => p.EMail == paciente.EMail && p.Id != paciente.Id, "Email já cadastrado");

        MemDB.Pacientes.Update(paciente);

        await Task.CompletedTask;
    }

    public async Task ExcluirPaciente(int id)
    {
        MemDB.Pacientes.CheckFK(id, "Paciente não encontrado");

        MemDB.Pacientes.Delete(id);

        await Task.CompletedTask;
    }
}
