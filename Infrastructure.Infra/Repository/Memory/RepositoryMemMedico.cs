using Domain.Entity;
using Domain.Interfaces.Repository;
using Force.DeepCloner;

namespace Infrastructure.Repository.Memory;

public class RepositoryMemMedico : IRepositoryMedico
{
    public async Task<Medico?> ResgatarMedicoPorId(int id) => await Task.FromResult(MemDB.Medicos.FirstOrDefault(p => p.Id == id)?.DeepClone());

    public async Task<Medico?> ResgatarMedicoPorEmail(string email) => await Task.FromResult(MemDB.Medicos.FirstOrDefault(p => p.EMail == email)?.DeepClone());

    public async Task RegistarNovoMedico(Medico medico)
    {
        ArgumentNullException.ThrowIfNull(medico);

        medico.Validar();

        MemDB.Medicos.CheckUK(m => m.CPF == medico.CPF, "CPF já cadastrado");
        MemDB.Medicos.CheckUK(m => m.EMail == medico.EMail, "Email já cadastrado");
        MemDB.Medicos.CheckUK(m => m.CRM == medico.CRM, "CRM já cadastrado");

        MemDB.Medicos.Insert(medico);

        await Task.CompletedTask;
    }

    public async Task AlterarDadosMedico(Medico medico)
    {
        ArgumentNullException.ThrowIfNull(medico);

        medico.Validar();

        MemDB.Medicos.CheckFK(medico.Id, "Médico não encontrado");

        MemDB.Medicos.CheckUK(m => m.CPF == medico.CPF && m.Id != medico.Id, "CPF já cadastrado");
        MemDB.Medicos.CheckUK(m => m.EMail == medico.EMail && m.Id != medico.Id, "Email já cadastrado");
        MemDB.Medicos.CheckUK(m => m.CRM == medico.CRM && m.Id != medico.Id, "CRM já cadastrado");

        MemDB.Medicos.Update(medico);

        await Task.CompletedTask;
    }

    public async Task ExcluirMedico(int id)
    {
        MemDB.Medicos.CheckFK(id, "Médico não encontrado");

        MemDB.Medicos.Delete(id);

        await Task.CompletedTask;
    }
}