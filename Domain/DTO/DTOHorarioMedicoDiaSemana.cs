using Domain.Entity;

namespace Domain.DTO;

public class DTOHorarioMedicoDiaSemana
{
    public int IdMedico { get; set; }
    public DayOfWeek DiaSemana { get; set; }
    public Periodo[]? Horarios { get; set; }
}
