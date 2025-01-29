using Domain.Entity;
using Domain.Interfaces.Repository;

namespace Service.Service;

public class ServiceHorarioMedico(IRepositoryMedico repositoryMedico, IRepositoryHorarioMedico repositoryHorarioMedico): IServiceHorarioMedico
{
    public HorarioMedico[] ListarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana) =>
        repositoryHorarioMedico.ListarHorariosMedicoDiaSemana(idMedico, diaSemana);

    public void RegistrarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana, params Periodo[] periodos)
    {
        ArgumentNullException.ThrowIfNull(periodos);

        if (periodos.Length == 0)
            throw new ArgumentException("Deve ser informado ao menos um periodo.");

        foreach (var periodo in periodos)
            periodo.Validar();

        var conflito = Periodo.ChecaConflitos(periodos);
        if (conflito != null)
            throw new ArgumentException($"Conflito de horario: {conflito.Value.periodo1} - {conflito.Value.periodo2}.");

        var medicoCadastro = repositoryMedico.ResgatarMedicoPorId(idMedico);
        if (medicoCadastro == null)
            throw new ArgumentException("Medico não encontrado.", nameof(idMedico));

        repositoryHorarioMedico.RegistrarHorariosMedicoDiaSemana(idMedico, diaSemana, periodos);
    }
}
