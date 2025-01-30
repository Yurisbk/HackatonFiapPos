using Domain.DTO;

namespace Domain.Interfaces.Service;

public interface IServiceConsulta
{
    DTOHorariosLivre[] ListarHorariosLivres(int dias = 15);
    void RegistrarConsulta(int pacienteId, int medicoId, DateTime horario);
}