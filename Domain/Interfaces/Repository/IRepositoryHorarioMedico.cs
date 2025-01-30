using Domain.Entity;

namespace Domain.Interfaces.Repository;

public interface IRepositoryHorarioMedico
{
    void RegistrarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana, params Periodo[] periodos);
    HorarioMedico[] ListarHorariosMedicoDiaSemana(DayOfWeek diaSemana);
}
