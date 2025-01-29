using Domain.Entity;

namespace Service.Service;

public interface IServiceHorarioMedico
{
    HorarioMedico[] ListarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana);
    void RegistrarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana, params Periodo[] periodos);
}