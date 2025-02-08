using Domain.DTO;
using ConsumerNotificacao.Service;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerNotificacao.Event
{
    public class EnviaEmail : IConsumer<DTONotificacao>
    {
        private readonly IEmailService _email;

        public EnviaEmail(IEmailService email)
        {
            _email = email;
        }
        public Task Consume(ConsumeContext<DTONotificacao> context)
        {
            _email.EnviarEmail(context.Message);
            Console.WriteLine($"Email enviado para o medico {context.Message.NomeMedico.ToString()} atualizado com sucesso");
            return Task.CompletedTask;
        }
    }
}
