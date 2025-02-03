using Domain.Interfaces;

namespace Domain.DTO;

public struct Periodo: IValidavel
{
    public TimeSpan HoraInicial;
    public TimeSpan HoraFinal;

    public TimeSpan Duracao => HoraFinal - HoraInicial;

    public Periodo(double horaInicial, double horaFinal)
    {
        HoraInicial = TimeSpan.FromHours(horaInicial);
        HoraFinal = TimeSpan.FromHours(horaFinal);
    }

    public void Validar()
    {
        if ((HoraInicial.TotalDays > 1) || (HoraFinal.TotalDays > 1))
            throw new Exception("Hora inicial e hora final devem estar entre as 24 horas do dia");

        if ((HoraInicial.TotalHours < 0) || (HoraFinal.TotalHours < 0))
            throw new Exception("Hora inválida");

        if (HoraInicial >= HoraFinal)
            throw new Exception("Hora inicial deve ser menor que o hora final.");
    }

    bool ChecaConflito(Periodo periodo)
    {
        if (HoraInicial >= periodo.HoraFinal)
            return false;

        if (HoraFinal <= periodo.HoraInicial)
            return false;

        return true;
    }

    public override string ToString() => $"{HoraInicial} - {HoraFinal}";

    static public (Periodo periodo1, Periodo periodo2)? ChecaConflitos(params Periodo[] periodos)
    {
        for (int a = 0; a < periodos.Length; a++)
        {
            for (int b = a + 1; b < periodos.Length; b++)
            {
                if (periodos[a].ChecaConflito(periodos[b]))
                    return (periodos[a], periodos[b]);
            }
        }

        return null;
    }
}

public class HorarioMedico : Entidade, IValidavel
{
    public int IdMedico;
    public DayOfWeek DiaSemana;
    public Periodo Periodo;

    public HorarioMedico()
    {
        
    }

    public HorarioMedico(DayOfWeek diaSemana, double horaInicial, double horaFinal)
    {
        DiaSemana = diaSemana;
        Periodo = new Periodo(horaInicial, horaFinal);
    }

    public void Validar() =>
        Periodo.Validar();
}