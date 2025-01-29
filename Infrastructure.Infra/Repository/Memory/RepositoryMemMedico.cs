using Domain.Entity;
using Domain.Interfaces.Repository;
using Force.DeepCloner;

namespace Infrastructure.Repository.Memory;

public class RepositoryMemMedico: IRepositorioMedico
{
    List<Medico> Medicos = new();

    public Medico? ResgatarMedicoPorId(int id) => Medicos.FirstOrDefault(p => p.Id == id)?.DeepClone();

    public Medico? ResgatarMedicoPorEmail(string email) => Medicos.FirstOrDefault(p => p.EMail == email)?.DeepClone();

    public void RegistarNovoMedico(Medico medico)
    {
        ArgumentNullException.ThrowIfNull(medico);

        medico.Validar();

        // Simula UKs
        if (Medicos.Any(p => p.CPF == medico.CPF))
            throw new ArgumentException("CPF já cadastrado");

        if (Medicos.Any(p => p.EMail == medico.EMail))
            throw new ArgumentException("Email já cadastrado");

        // Simula PK
        medico.Id = Medicos.Count == 0 ? 0 : Medicos.Max(p => p.Id) + 1;

        Medicos.Add(medico.DeepClone());
    }

    public void AlterarDadosMedico(Medico medico)
    {
        ArgumentNullException.ThrowIfNull(medico);

        medico.Validar();

        Medico? medicoCadastro = ResgatarMedicoPorId(medico.Id ?? -1);
        if (medicoCadastro == null)
            throw new ArgumentException("Médico não encontrado");

        // Simula UKs
        if (Medicos.FirstOrDefault(p => p.CPF == medico.CPF) != medicoCadastro)
            throw new ArgumentException("CPF já cadastrado");

        if (Medicos.FirstOrDefault(p => p.EMail == medico.EMail) != medicoCadastro)
            throw new ArgumentException("Email já cadastrado");

        Medicos[Medicos.IndexOf(medicoCadastro)] = medico.DeepClone();
    }

    public void ExcluirMedico(int id)
    {
        Medico? medicoCadastro = ResgatarMedicoPorId(id);
        if (medicoCadastro == null)
            throw new ArgumentException("Médico não encontrado");

        Medicos.RemoveAt(Medicos.FirstOrDefault(p => p.Id == id)?.Id ?? -1);

    }
}
