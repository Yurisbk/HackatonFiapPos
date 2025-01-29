using Domain.Interfaces;

namespace Domain.Entity;

public class HorarioMedico : Entidade, IValidavel
{
    public DayOfWeek DiaSemana;
    public TimeSpan HorarioInicio;
    public TimeSpan HorarioFim;

    public HorarioMedico()
    {
        
    }

    public HorarioMedico(DayOfWeek diaSemana, double horaInicial, double horaFinal)
    {
        DiaSemana = diaSemana;
        HorarioInicio = TimeSpan.FromHours(horaInicial);
        HorarioFim = TimeSpan.FromHours(horaFinal);
    }

    public void Validar()
    {
        if (HorarioInicio >= HorarioFim)
            throw new Exception("Horário de início deve ser menor que o horário de fim.");
    }
}
