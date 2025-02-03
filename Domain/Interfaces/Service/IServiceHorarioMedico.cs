using Domain.DTO;

namespace Domain.Interfaces.Service;

public interface IServiceHorarioMedico
{
    HorarioMedico[] ListarHorariosMedicoDiaSemana(DayOfWeek diaSemana);
    void RegistrarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana, params Periodo[] periodos);
}