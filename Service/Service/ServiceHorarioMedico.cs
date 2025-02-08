using Domain.Entity;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Service.Helper;

namespace Service.Service;

public class ServiceHorarioMedico(IRepositoryMedico repositoryMedico, IRepositoryHorarioMedico repositoryHorarioMedico, HelperTransacao helperTransacao) : IServiceHorarioMedico
{
    public async Task RegistrarHorariosMedicoDiaSemana(int idMedico, DayOfWeek diaSemana, params Periodo[] periodos)
    {
        using (var transcao = helperTransacao.CriaTransacao())
        {
            ArgumentNullException.ThrowIfNull(periodos);

            if (periodos.Length == 0)
                throw new ArgumentException("Deve ser informado ao menos um periodo.");

            foreach (var periodo in periodos)
                periodo.Validar();

            // Checa conflitos de horario
            var conflito = Periodo.ChecaConflitos(periodos);
            if (conflito != null)
                throw new ArgumentException($"Conflito de horario: {conflito.Value.periodo1} - {conflito.Value.periodo2}.");

            var medicoCadastro = repositoryMedico.ResgatarMedicoPorId(idMedico);
            if (medicoCadastro == null)
                throw new ArgumentException("Medico não encontrado.", nameof(idMedico));

            await repositoryHorarioMedico.RegistrarHorariosMedicoDiaSemana(idMedico, diaSemana, periodos);
        }
    }

    public async Task<HorarioMedico[]> ResgatarHorariosMedicoDiaSemana(int idMedico, DayOfWeek dayOfWeek) =>
        await repositoryHorarioMedico.ResgatarHorariosMedicoDiaSemana(idMedico, dayOfWeek);
}
