using Domain.DTO;

namespace Domain.Interfaces.Service;

public interface IServiceConsulta
{
    Task<DTOHorariosLivre[]> ListarHorariosLivres(int dias = 15);
    Task RegistrarConsulta(int pacienteId, int medicoId, DateTime horario);
}