using Domain.Entity;

namespace Domain.Interfaces.Repository;

public interface IRepositoryMemConsulta
{
    void RegistrarConsulta(Consulta consulta);
    Consulta[] ListarProximasConsultas(int dias = 15);
}