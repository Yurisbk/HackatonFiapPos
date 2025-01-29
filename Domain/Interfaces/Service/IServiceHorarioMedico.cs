using Domain.Entity;

namespace Domain.Interfaces.Service;

public interface IServiceHorarioMedico
{
    HorarioMedico[] ListarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana);
    void RegistrarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana, params Periodo[] periodos);
}