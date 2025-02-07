using Domain.Entity;
using Domain.Interfaces.Repository;
using Force.DeepCloner;

namespace Infrastructure.Repository.Memory;

public class RepositoryMemMedico : IRepositoryMedico
{
    public async Task<Medico?> ResgatarMedicoPorId(int id)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            return MemDB.Medicos.FirstOrDefault(p => p.Id == id)?.DeepClone();
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task<Medico?> ResgatarMedicoPorEmail(string email)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            return MemDB.Medicos.FirstOrDefault(p => p.EMail == email)?.DeepClone();
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task RegistarNovoMedico(Medico medico)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            ArgumentNullException.ThrowIfNull(medico);

            medico.Validar();

            MemDB.Medicos.CheckUK(m => m.CPF == medico.CPF, "CPF já cadastrado");
            MemDB.Medicos.CheckUK(m => m.EMail == medico.EMail, "Email já cadastrado");
            MemDB.Medicos.CheckUK(m => m.CRM == medico.CRM, "CRM já cadastrado");

            MemDB.Medicos.Insert(medico);
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task AlterarDadosMedico(Medico medico)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            ArgumentNullException.ThrowIfNull(medico);

            medico.Validar();

            MemDB.Medicos.CheckFK(medico.Id, "Médico não encontrado");

            MemDB.Medicos.CheckUK(m => m.CPF == medico.CPF && m.Id != medico.Id, "CPF já cadastrado");
            MemDB.Medicos.CheckUK(m => m.EMail == medico.EMail && m.Id != medico.Id, "Email já cadastrado");
            MemDB.Medicos.CheckUK(m => m.CRM == medico.CRM && m.Id != medico.Id, "CRM já cadastrado");

            MemDB.Medicos.Update(medico);
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task ExcluirMedico(int id)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            MemDB.Medicos.CheckFK(id, "Médico não encontrado");

            MemDB.Medicos.Delete(id);
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }
}