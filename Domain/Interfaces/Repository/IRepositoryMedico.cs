using Domain.Entity;

namespace Domain.Interfaces.Repository;

public interface IRepositorioMedico
{
    Medico? ResgatarMedicoPorId(int id);
    Medico? ResgatarMedicoPorEmail(string email);
    void RegistarNovoMedico(Medico Medico);
    void AlterarDadosMedico(Medico Medico);
    void ExcluirMedico(int id);
}
