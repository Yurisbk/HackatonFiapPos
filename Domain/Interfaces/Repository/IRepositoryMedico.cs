using Domain.Entity;

namespace Domain.Interfaces.Repository;

public interface IRepositoryMedico
{
    Task<Medico?> ResgatarMedicoPorId(int id);
    Task<Medico?> ResgatarMedicoPorCRM(string email);
    Task RegistarNovoMedico(Medico Medico);
    Task AlterarDadosMedico(Medico Medico);
    Task ExcluirMedico(int id);
    Task<string[]> ListarEspecialidadesMedicas();
    Task<Medico[]> ListarMedicosDisponiveisNaEspecialidade(string especialidade);
}
