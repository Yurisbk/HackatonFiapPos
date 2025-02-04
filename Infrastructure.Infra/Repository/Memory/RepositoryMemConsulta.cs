using Domain.Entity;
using Domain.Interfaces.Repository;

namespace Infrastructure.Repository.Memory;

public class RepositoryMemConsulta : IRepositoryConsulta
{
    public async Task RegistrarConsulta(Consulta consulta)
    {
        ArgumentNullException.ThrowIfNull(consulta);

        // Simula UK
        if (MemDB.Consultas.Any(c => c.IdMedico == consulta.IdMedico && c.DataHora == consulta.DataHora))
            throw new InvalidOperationException("Medico ja tem consulta marcada neste horário.");

        // Simula FK
        if (!MemDB.Medicos.Any(m => m.Id == consulta.IdMedico))
            throw new InvalidOperationException("Medico não encontrado.");

        if (!MemDB.Pacientes.Any(p => p.Id == consulta.IdPaciente))
            throw new InvalidOperationException("Paciente não encontrado.");

        // Simula PK
        consulta.Id = MemDB.CriaChaveUnica(MemDB.Consultas);

        MemDB.Consultas.Add(consulta);            

        await Task.CompletedTask;
    }

    public async Task<Consulta[]> ListarProximasConsultas(int dias = 15)
    {
        return await Task.FromResult(MemDB.Consultas
        .Where(c =>
            c.DataHora.Date >= DateTime.Now.Date &&
            c.DataHora.Date <= DateTime.Now.Date.AddDays(dias))
        .ToArray());
    }
}
