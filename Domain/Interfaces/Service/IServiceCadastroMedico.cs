using Domain.Entity;

namespace Domain.Interfaces.Service;

public interface IServiceCadastroMedico
{
    Task ExcluirMedico(int id);
    Task GravarMedico(Medico medico);
    Task<Medico?> ResgatarMedicoPorEmail(string email);
}
