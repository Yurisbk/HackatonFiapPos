using Domain.Entity;

namespace Domain.Interfaces.Service;

public interface IServiceCadastroMedico
{
    Task ExcluirMedico(int id);
    Task GravarMedico(Medico medico);
    Task<Medico?> ResgatarMedicoPorEmail(string email);
    Task<Medico[]> ListarMedicos(string especialidade, DayOfWeek? atendeDiaSemana);
    Task<string[]> ListarEspecialidadeMedicas();
}
