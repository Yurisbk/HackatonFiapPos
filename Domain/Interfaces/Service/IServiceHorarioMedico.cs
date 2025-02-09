using Domain.Entity;

namespace Domain.Interfaces.Service;

public interface IServiceHorarioMedico
{
    Task RegistrarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana, params Periodo[] periodos);
    Task<HorarioMedico[]> ResgatarHorariosMedicoDiaSemana(int idMedico, DayOfWeek dayOfWeek);
}