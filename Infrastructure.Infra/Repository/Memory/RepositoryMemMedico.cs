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

        // Simula UKs

        if (MemDB.Medicos.Any(m => m.CPF == medico.CPF))
            throw new ArgumentException("CPF já cadastrado");

        if (MemDB.Medicos.Any(m => m.EMail == medico.EMail))
            throw new ArgumentException("Email já cadastrado");

        if (MemDB.Medicos.Any(m => m.CRM == medico.CRM))
            throw new ArgumentException("Email já cadastrado");

        // Simula PK
        medico.Id = MemDB.CriaChaveUnica(MemDB.Medicos);

        MemDB.Medicos.Add(medico.DeepClone());

        await Task.CompletedTask;
    }

    public async Task AlterarDadosMedico(Medico medico)
    {
        ArgumentNullException.ThrowIfNull(medico);

        medico.Validar();

        Medico? medicoCadastro = await ResgatarMedicoPorId(medico.Id ?? -1);
        if (medicoCadastro == null)
            throw new ArgumentException("Médico não encontrado");

        // Simula UKs

        if (MemDB.Medicos.FirstOrDefault(m => m.CPF == medico.CPF) != medicoCadastro)
            throw new ArgumentException("CPF já cadastrado");

        if (MemDB.Medicos.FirstOrDefault(m => m.EMail == medico.EMail) != medicoCadastro)
            throw new ArgumentException("Email já cadastrado");

        if (MemDB.Medicos.FirstOrDefault(m => m.CRM == medico.CRM) != medicoCadastro)
            throw new ArgumentException("CRM já cadastrado");

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