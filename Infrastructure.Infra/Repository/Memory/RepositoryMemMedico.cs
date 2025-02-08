using Domain.DTO;
using Domain.Interfaces.Repository;
using Force.DeepCloner;

namespace Infrastructure.Repository.Memory;

public class RepositoryMemMedico: IRepositoryMedico
{
    public Medico? ResgatarMedicoPorId(int id) => MemDB.Medicos.FirstOrDefault(p => p.Id == id)?.DeepClone();

    public Medico? ResgatarMedicoPorEmail(string email) => MemDB.Medicos.FirstOrDefault(p => p.EMail == email)?.DeepClone();
    public Medico? ResgatarMedicoPorCRM(string crm) => MemDB.Medicos.FirstOrDefault(p => p.CRM == crm)?.DeepClone();

    public void RegistarNovoMedico(Medico medico)
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
    }

    public void AlterarDadosMedico(Medico medico)
    {
        ArgumentNullException.ThrowIfNull(medico);

        medico.Validar();

        Medico? medicoCadastro = ResgatarMedicoPorId(medico.Id ?? -1);
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
    }

    public void ExcluirMedico(int id)
    {
        Medico? medicoCadastro = ResgatarMedicoPorId(id);
        if (medicoCadastro == null)
            throw new ArgumentException("Médico não encontrado");

        MemDB.Medicos.RemoveAt(MemDB.Medicos.FirstOrDefault(medico => medico.Id == id)?.Id ?? -1);
    }
}
