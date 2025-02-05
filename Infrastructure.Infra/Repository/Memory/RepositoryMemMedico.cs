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

        MemDB.SimulaUK(MemDB.Medicos, (a, b) => a.CPF == b.CPF, medico, "CPF já cadastrado");
        MemDB.SimulaUK(MemDB.Medicos, (a, b) => a.EMail == b.EMail, medico, "Email já cadastrado");
        MemDB.SimulaUK(MemDB.Medicos, (a, b) => a.CRM == b.CRM, medico, "CRM já cadastrado");

        // Simula PK
        medico.Id = MemDB.CriaChaveUnica(MemDB.Medicos);

        MemDB.Medicos.Add(medico.DeepClone());

        await Task.CompletedTask;
    }

    public async Task AlterarDadosMedico(Medico medico)
    {
        ArgumentNullException.ThrowIfNull(medico);

        medico.Validar();

        Medico? medicoCadastro = MemDB.Medicos.FirstOrDefault(p => p.Id == medico.Id);
        if (medicoCadastro == null)
            throw new ArgumentException("Médico não encontrado");

        MemDB.SimulaUK(MemDB.Medicos, (a, b) => a.CPF == b.CPF && a.Id != b.Id, medico, "CPF já cadastrado");
        MemDB.SimulaUK(MemDB.Medicos, (a, b) => a.EMail == b.EMail && a.Id != b.Id, medico, "Email já cadastrado");
        MemDB.SimulaUK(MemDB.Medicos, (a, b) => a.CRM == b.CRM && a.Id != b.Id, medico, "CRM já cadastrado");

        MemDB.Medicos[MemDB.Medicos.IndexOf(medicoCadastro)] = medico.DeepClone();

        await Task.CompletedTask;
    }

    public async Task ExcluirMedico(int id)
    {
        Medico? medicoCadastro = await ResgatarMedicoPorId(id);
        if (medicoCadastro == null)
            throw new ArgumentException("Médico não encontrado");

        MemDB.Medicos.RemoveAt(MemDB.Medicos.FirstOrDefault(medico => medico.Id == id)?.Id ?? -1);

        await Task.CompletedTask;
    }
}