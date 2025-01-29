using Domain.Entidades;
using Domain.Interfaces;

namespace Medico.Domain.Entitidades;

public class HorarioMedico: Entidade, IValidavel
{
    public DayOfWeek DiaSemana;
    public TimeSpan HorarioInicio;
    public TimeSpan HorarioFim;

    public void Validar()
    {
        if (HorarioInicio >= HorarioFim)
            throw new Exception("Horário de início deve ser menor que o horário de fim.");
    }
}
