using Domain.Entity;
using Domain.Interfaces.Repository;

namespace Infrastructure.Repository.Memory;

public class RepositoryMemHorarioMedico : IRepositoryHorarioMedico
{
    public async Task RegistrarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana, params Periodo[] periodos)
    {
        ArgumentNullException.ThrowIfNull(periodos);

        // Simula FK
        if (!MemDB.Medicos.Any(m => m.Id == idMedico))
            throw new InvalidOperationException("Medico não encontrado.");

        if (periodos.Length == 0)
            throw new ArgumentException("Deve ser informado ao menos um periodo.");

        foreach (var periodo in periodos)
            periodo.Validar();

        // Exclui todos os horarios do dia da semana do medico
        MemDB.HorariosMedicos.RemoveAll(h => h.IdMedico == idMedico && h.DiaSemana == diaSemana);

        // Adiciona horarios para os periodos
        foreach (var periodo in periodos)
            MemDB.HorariosMedicos.Add(new HorarioMedico() { Id = MemDB.CriaChaveUnica(MemDB.HorariosMedicos), IdMedico = idMedico, DiaSemana = diaSemana, Periodo = periodo });

        await Task.CompletedTask;
    }

    public async Task<HorarioMedico[]> ListarHorariosMedicoDiaSemana(DayOfWeek diaSemana)
    {
        return await Task.FromResult(MemDB.HorariosMedicos
        .Where(horario => horario.DiaSemana == diaSemana)
        .OrderBy(horario => horario.IdMedico)
        .ThenBy(horario => horario.Periodo.HoraInicial)
        .ToArray());
    }
}