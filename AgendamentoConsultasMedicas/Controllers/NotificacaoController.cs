using MassTransit.Transports.Fabric;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;   
using System.Diagnostics;
using Domain.Interfaces.Service;
using Domain.DTO;

namespace AgendamentoConsultasMedicas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacaoController : ControllerBase
    {
        private readonly IServiceNotificacao _notifica;

        public NotificacaoController(IServiceNotificacao notifica) 
        {
            _notifica = notifica;
        }

        [HttpGet("SendEmail")]
        public async Task<IActionResult> GetAllContacts()
        {
            DTONotificacao Notificacao = new DTONotificacao
            {
                EmailMedico = "atendimentoconsultashackaton@gmail.com",

                EmailPaciente = "atendimentoconsultashackaton@gmail.com",

                NomePaciente = "Faustão Silva",

                NomeMedico = "House",

                HorarioConsulta = "03/03/2025 04:00:00",

                Confirmacao = false

            };

            await _notifica.EnviaNotificacaoMedico(Notificacao);
            
            return Ok();
        }
        [Authorize]
        [HttpGet("ValidaToken")]
        public async Task<IActionResult> ValidaToken()
        {
           return Ok("Token válidado com sucesso!");
        }
    }
}
