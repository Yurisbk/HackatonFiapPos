using Domain.Entity;
using Domain.Interfaces.Repository;

namespace Infrastructure.Repository.Memory;

public class RepositoryMemHorarioMedico : IRepositoryHorarioMedico
{
    public async Task RegistrarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana, params Periodo[] periodos)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            ArgumentNullException.ThrowIfNull(periodos);

            MemDB.Medicos.CheckFK(idMedico, "Medico não encontrado.");

            if (periodos.Length == 0)
                throw new ArgumentException("Deve ser informado ao menos um periodo.");

            foreach (var periodo in periodos)
                periodo.Validar();

            // Checa conflitos de horario
            var conflito = Periodo.ChecaConflitos(periodos);
            if (conflito != null)
                throw new ArgumentException($"Conflito de horario: {conflito.Value.periodo1} - {conflito.Value.periodo2}.");

            // Exclui todos os horarios do dia da semana do medico
            MemDB.HorariosMedicos.RemoveAll(h => h.IdMedico == idMedico && h.DiaSemana == diaSemana);

            // Adiciona horarios para os periodos
            foreach (var periodo in periodos)
                MemDB.HorariosMedicos.Insert(new HorarioMedico() { IdMedico = idMedico, DiaSemana = diaSemana, Periodo = periodo });
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task<HorarioMedico[]> ListarHorariosMedicoDiaSemana(DayOfWeek diaSemana)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            return MemDB.HorariosMedicos
                .Where(horario => horario.DiaSemana == diaSemana)
                .OrderBy(horario => horario.IdMedico)
                .ThenBy(horario => horario.Periodo.HoraInicial)
                .ToArray();
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }
}