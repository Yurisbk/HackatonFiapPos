using Domain.Entity;
using Domain.Interfaces.Repository;

namespace Infrastructure.Repository.Memory;

public class RepositoryMemHorarioMedico: IRepositoryHorarioMedico
{
    public void RegistrarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana, params Periodo[] periodos)
    {
        ArgumentNullException.ThrowIfNull(periodos);

        if (periodos.Length == 0)
            throw new ArgumentException("Deve ser informado ao menos um periodo.");

        foreach (var periodo in periodos)
            periodo.Validar();

        // Exclui todos os horarios do dia da semana do medico
        MemDB.HorariosMedicos.RemoveAll(h => h.IdMedico == idMedico && h.DiaSemana == diaSemana);

        // Adiciona horarios para os periodos
        foreach (var periodo in periodos)
            MemDB.HorariosMedicos.Add(new HorarioMedico() { Id = MemDB.CriaChaveUnica(MemDB.HorariosMedicos), IdMedico = idMedico, DiaSemana = diaSemana, Periodo = periodo} );
    }

    public HorarioMedico[] ListarHorariosMedicoDiaSemana(DayOfWeek diaSemana)
        => MemDB.HorariosMedicos
            .Where(horario => horario.DiaSemana == diaSemana)
            .OrderBy(horario => horario.IdMedico)
            .ThenBy(horario => horario.Periodo.HoraInicial)
            .ToArray();
}