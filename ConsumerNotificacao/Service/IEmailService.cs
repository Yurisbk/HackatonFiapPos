using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerNotificacao.Service
{
    public interface IEmailService
    {
        void EnviarEmail(DTONotificacao notificacao);
    }
}
