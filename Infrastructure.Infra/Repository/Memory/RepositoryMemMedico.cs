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

        MemDB.Medicos.UK(m => m.CPF == medico.CPF, "CPF já cadastrado");
        MemDB.Medicos.UK(m => m.EMail == medico.EMail, "Email já cadastrado");
        MemDB.Medicos.UK(m => m.CRM == medico.CRM, "CRM já cadastrado");

        MemDB.Medicos.PK(medico);

        MemDB.Medicos.Add(medico.DeepClone());

        await Task.CompletedTask;
    }

    public async Task AlterarDadosMedico(Medico medico)
    {
        ArgumentNullException.ThrowIfNull(medico);

        medico.Validar();

        MemDB.Medicos.FK(medico.Id, "Médico não encontrado");

        MemDB.Medicos.UK(m => m.CPF == medico.CPF && m.Id != medico.Id, "CPF já cadastrado");
        MemDB.Medicos.UK(m => m.EMail == medico.EMail && m.Id != medico.Id, "Email já cadastrado");
        MemDB.Medicos.UK(m => m.CRM == medico.CRM && m.Id != medico.Id, "CRM já cadastrado");

        MemDB.Medicos[MemDB.Medicos.IndexOfEntity(medico)] = medico.DeepClone();

        await Task.CompletedTask;
    }

    public async Task ExcluirMedico(int id)
    {
        MemDB.Medicos.FK(id, "Médico não encontrado");

        MemDB.Medicos.RemoveAt(MemDB.Medicos.IndexOfId(id));

        await Task.CompletedTask;
    }
}