using Domain.DTO;

namespace Domain.Interfaces.Service;

public interface IServiceCadastroMedico
{
    void ExcluirMedico(int id);
    void GravarMedico(Medico medico);
    Medico? ResgatarMedicoPorEmail(string email);
    Medico? ResgatarMedicoPorCRM(string crm);
    Task<DTOAutenticacaoResponse?> LoginMedico(DTOLoginMedico loginRequest);
    Task<DTOCreateUsuarioResponse?> CriarMedico(DTOCreateMedico createPaciente);
}
