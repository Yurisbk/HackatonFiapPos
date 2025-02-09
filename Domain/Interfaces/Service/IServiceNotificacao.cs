using Domain.DTO;

namespace Domain.Interfaces.Service;

public interface IServiceNotificacao
{ 
    Task EnviaNotificacao(DTONotificacao notificacao);
}
