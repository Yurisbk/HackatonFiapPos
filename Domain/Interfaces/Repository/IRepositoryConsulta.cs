using Domain.DTO;

namespace Domain.Interfaces.Repository;

public interface IRepositoryConsulta
{
    void RegistrarConsulta(Consulta consulta);
    Consulta[] ListarProximasConsultas(int dias = 15);
}