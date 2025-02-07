using Domain.Entity;
using Domain.Interfaces.Repository;

namespace Infrastructure.Repository.Memory;

public class RepositoryMemConsulta : IRepositoryConsulta
{
    public async Task RegistrarConsulta(Consulta consulta)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            ArgumentNullException.ThrowIfNull(consulta);

            MemDB.Consultas.CheckUK(c => c.IdMedico == consulta.IdMedico && c.DataHora == consulta.DataHora, "Medico ja tem consulta marcada neste horário.");

            MemDB.Medicos.CheckFK(consulta.IdMedico, "Medico não encontrado.");
            MemDB.Pacientes.CheckFK(consulta.IdPaciente, "Paciente não encontrado.");

            MemDB.Consultas.Insert(consulta);
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task<Consulta[]> ListarProximasConsultas(int dias = 15)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            return MemDB.Consultas
            .Where(c =>
                c.DataHora.Date >= DateTime.Now.Date &&
                c.DataHora.Date <= DateTime.Now.Date.AddDays(dias))
            .ToArray();
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }
}
