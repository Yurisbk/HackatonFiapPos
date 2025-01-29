using Domain.Entity;
using Domain.Interfaces.Repository;

namespace Infrastructure.Repository.Memory;

public class RepositoryMemHorarioMedico: IRepositoryHorarioMedico
{
    List<HorarioMedico> Horarios = new();

    public void RegistrarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana, params Periodo[] periodos)
    {
        ArgumentNullException.ThrowIfNull(periodos);

        if (periodos.Length == 0)
            throw new ArgumentException("Deve ser informado ao menos um periodo.");

        foreach (var periodo in periodos)
            periodo.Validar();

        // Exclui todos os horarios do dia da semana do medico
        Horarios.RemoveAll(h => h.IdMedico == idMedico && h.DiaSemana == diaSemana);

        // Adiciona horarios para os periodos
        foreach (var periodo in periodos)
            Horarios.Add(new HorarioMedico() { Id = Horarios.Max(h => h.Id!) + 1, IdMedico = idMedico, DiaSemana = diaSemana, Periodo = periodo} );
    }

    public HorarioMedico[] ListarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana)
        => Horarios
            .Where(horario => horario.IdMedico == idMedico && horario.DiaSemana == diaSemana)
            .OrderBy(horario => horario.Periodo.HoraInicial)
            .ToArray();
}