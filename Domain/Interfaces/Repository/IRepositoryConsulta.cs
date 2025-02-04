using Domain.Entity;

namespace Domain.Interfaces.Repository;

public interface IRepositoryConsulta
{
    Task RegistrarConsulta(Consulta consulta);
    Task<Consulta[]> ListarProximasConsultas(int dias = 15);
}