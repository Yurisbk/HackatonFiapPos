using Domain.DTO;

namespace Domain.Interfaces.Service;

public interface IServiceCadastroMedico
{
    void ExcluirMedico(int id);
    void GravarMedico(Medico medico);
    Medico? ResgatarMedicoPorEmail(string email);
}
