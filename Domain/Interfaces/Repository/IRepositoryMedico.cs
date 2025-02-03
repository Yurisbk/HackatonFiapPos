using Domain.DTO;

namespace Domain.Interfaces.Repository;

public interface IRepositoryMedico
{
    Medico? ResgatarMedicoPorId(int id);
    Medico? ResgatarMedicoPorEmail(string email);
    void RegistarNovoMedico(Medico Medico);
    void AlterarDadosMedico(Medico Medico);
    void ExcluirMedico(int id);
}
