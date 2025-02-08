using Domain.Entity;

namespace Domain.Interfaces.Repository;

public interface IRepositoryHorarioMedico
{
    Task RegistrarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana, params Periodo[] periodos);
    Task<HorarioMedico[]> ResgatarHorariosMedicoDiaSemana(int idMedico, DayOfWeek dayOfWeek);
}
