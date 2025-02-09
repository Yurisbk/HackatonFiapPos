using Domain.Interfaces;

namespace Domain.Entity;

public struct Periodo: IValidavel
{
    public int HoraInicial { get; set; }
    public int HoraFinal { get; set; }

    public int Duracao => HoraFinal - HoraInicial;

    public Periodo(int horaInicial, int horaFinal)
    {
        HoraInicial = horaInicial;
        HoraFinal = horaFinal;
    }

    public void Validar()
    {
        if ((HoraInicial > 23) || (HoraFinal > 23))
            throw new Exception("Hora inicial e hora final devem estar entre as 24 horas do dia");

        if ((HoraInicial < 0) || (HoraFinal < 0))
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

    public bool ContemHora(int hora) => (HoraInicial <= hora) && (HoraFinal >= hora);

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
    public int IdMedico { get; set; }
    public DayOfWeek DiaSemana { get; set; }
    public Periodo Periodo { get; set; }

    public HorarioMedico()
    {
        
    }

    public HorarioMedico(DayOfWeek diaSemana, int horaInicial, int horaFinal)
    {
        DiaSemana = diaSemana;
        Periodo = new Periodo(horaInicial, horaFinal);
    }

    public void Validar() =>
        Periodo.Validar();
}