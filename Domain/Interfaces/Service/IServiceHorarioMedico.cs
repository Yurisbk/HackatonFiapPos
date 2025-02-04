using Domain.Entity;

namespace Domain.Interfaces.Service;

public interface IServiceHorarioMedico
{
    Task<HorarioMedico[]> ListarHorariosMedicoDiaSemana(DayOfWeek diaSemana);
    Task RegistrarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana, params Periodo[] periodos);
}