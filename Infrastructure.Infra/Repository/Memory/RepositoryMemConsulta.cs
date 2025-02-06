using Domain.Entity;
using Domain.Interfaces.Repository;

namespace Infrastructure.Repository.Memory;

public class RepositoryMemConsulta : IRepositoryConsulta
{
    public async Task RegistrarConsulta(Consulta consulta)
    {
        ArgumentNullException.ThrowIfNull(consulta);

        MemDB.Consultas.CheckUK(c => c.IdMedico == consulta.IdMedico && c.DataHora == consulta.DataHora, "Medico ja tem consulta marcada neste horário.");

        MemDB.Medicos.CheckFK(consulta.IdMedico, "Medico não encontrado.");
        MemDB.Pacientes.CheckFK(consulta.IdPaciente, "Paciente não encontrado.");

        MemDB.Consultas.Insert(consulta);         

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
