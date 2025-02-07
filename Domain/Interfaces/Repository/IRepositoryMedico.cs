using Domain.Entity;

namespace Domain.Interfaces.Repository;

public interface IRepositoryMedico
{
    Task<Medico?> ResgatarMedicoPorId(int id);
    Task<Medico?> ResgatarMedicoPorEmail(string email);
    Task RegistarNovoMedico(Medico Medico);
    Task AlterarDadosMedico(Medico Medico);
    Task ExcluirMedico(int id);
    Task<Medico[]> ListarMedicosPorEspecialidade(string especialidade);
    Task<string[]> ListarEspecialidadesMedicas();
}
