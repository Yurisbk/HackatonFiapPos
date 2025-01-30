using Domain.Entity;
using Domain.Interfaces.Repository;

namespace Infrastructure.Repository.Memory;

public class RepositoryMemConsulta : IRepositoryConsulta
{
    public void RegistrarConsulta(Consulta consulta)
    {
        ArgumentNullException.ThrowIfNull(consulta);

        // Simula UK
        if (MemDB.Consultas.Any(c => c.IdMedico == consulta.IdMedico && c.DataHora == consulta.DataHora))
            throw new InvalidOperationException("Medico ja tem consulta marcada neste horário.");

        // Simula PK
        consulta.Id = MemDB.CriaChaveUnica(MemDB.Consultas);

        MemDB.Consultas.Add(consulta);
    }

    public Consulta[] ListarProximasConsultas(int dias = 15) => 
        MemDB.Consultas
            .Where(c => 
                c.DataHora.Date >= DateTime.Now.Date && 
                c.DataHora.Date <= DateTime.Now.Date.AddDays(dias))
            .ToArray();
}
