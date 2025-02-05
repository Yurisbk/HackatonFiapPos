using Domain.DTO.Autenticacao;
using Domain.DTO;

namespace Service.Service;

public interface IServiceCadastroUsuario
{
    Task<DTOAutenticacaoResponse> RealizarLogin(DTOLoginUsuario loginUsuario);
    Task<DTOCreateUsuarioResponse> CriarUsuario(DTOCreatePessoa createPaciente);
}