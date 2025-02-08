namespace Infrastructure.Repository.Memory;

using Domain.Entity;
using Domain.Interfaces.Repository;
using Force.DeepCloner;

public class RepositoryMemPaciente : IRepositoryPaciente
{
    public async Task<Paciente?> ResgatarPacientePorId(int id)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            return MemDB.Pacientes.FirstOrDefault(p => p.Id == id)?.DeepClone();
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task<Paciente?> ResgatarPacientePorEmail(string email)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            return MemDB.Pacientes.FirstOrDefault(p => p.EMail == email)?.DeepClone();
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task<Paciente?> ResgatarPacientePorCpf(string cpf)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            return MemDB.Pacientes.FirstOrDefault(p => p.CPF == cpf)?.DeepClone();
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task RegistarNovoPaciente(Paciente paciente)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            ArgumentNullException.ThrowIfNull(paciente);

            paciente.Validar();

            MemDB.Pacientes.CheckUK(p => p.CPF == paciente.CPF, "CPF já cadastrado");
            MemDB.Pacientes.CheckUK(p => p.EMail == paciente.EMail, "Email já cadastrado");

            MemDB.Pacientes.Insert(paciente);
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task AlterarDadosPaciente(Paciente paciente)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            ArgumentNullException.ThrowIfNull(paciente);

            paciente.Validar();

            MemDB.Pacientes.CheckFK(paciente.Id, "Paciente não encontrado");

            MemDB.Pacientes.CheckUK(p => p.CPF == paciente.CPF && p.Id != paciente.Id, "CPF já cadastrado");
            MemDB.Pacientes.CheckUK(p => p.EMail == paciente.EMail && p.Id != paciente.Id, "Email já cadastrado");

            MemDB.Pacientes.Update(paciente);
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task ExcluirPaciente(int id)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            MemDB.Pacientes.CheckFK(id, "Paciente não encontrado");

            MemDB.Pacientes.Delete(id);
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }
}
