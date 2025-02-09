using Domain.DTO;
using Domain.Interfaces.Service;
using MassTransit;

namespace Service.Service
{
    public class ServiceNotificacao : IServiceNotificacao
    {
        private readonly IBus _bus;
        private readonly IRequestClient<DTONotificacao> _requestClient;

        public ServiceNotificacao(IBus bus, IRequestClient<DTONotificacao> requestClient)
        {
            _bus = bus;
            _requestClient = requestClient;
        }

        public async Task EnviaNotificacao(DTONotificacao notificacao)
        {

            var nomeFila = "Notificacao";
            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));
            await endpoint.Send(notificacao);
        }
    }
}