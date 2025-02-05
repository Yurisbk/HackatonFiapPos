using Domain.DTO;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task EnviaNotificacaoMedico(DTONotificacao notificacao)
        {

            var nomeFila = "Notificacao";
            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));
            await endpoint.Send(notificacao);
        }
    }
}
