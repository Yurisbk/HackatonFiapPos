using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;

namespace ConsumerNotificacao.Service
{

    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com"; 
        private readonly int _smtpPort = 587;
        private readonly string _emailRemetente = "atendimentoconsultashackaton@gmail.com"; 
        private readonly string _senhaEmail = "aszd lohi ccgu uout"; 

        public void EnviarEmail(DTONotificacao notificacao)
        {
            if (notificacao == null || string.IsNullOrWhiteSpace(notificacao.EmailMedico))
                throw new ArgumentException("Dados da notificação inválidos!");

            string assunto = "Health&Med - Nova consulta agendada";
            string corpo = GerarConteudoEmail(notificacao);

            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_emailRemetente, _senhaEmail);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailRemetente),
                    Subject = assunto,
                    Body = corpo,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(notificacao.EmailMedico);

                client.Send(mailMessage);
            }
        }

        private string GerarConteudoEmail(DTONotificacao notificacao)
        {
            string linkAgenda = GerarLinkAgenda(notificacao);

            return $@"
            <html>
            <body>
                <h2>Olá, Dr. {notificacao.NomeMedico}!</h2>
                <p>Você tem uma nova consulta marcada!</p>
                <p><strong>Paciente:</strong> {notificacao.NomePaciente}</p>
                <p><strong>Data e horário:</strong> {notificacao.HorarioConsulta}</p>
                <p><a href='{linkAgenda}' target='_blank'>Clique aqui para adicionar à sua agenda</a></p>
            </body>
            </html>";
        }


        private string GerarLinkAgenda(DTONotificacao notificacao)
        {
            string titulo = Uri.EscapeDataString("Consulta com " + notificacao.NomePaciente);
            string descricao = Uri.EscapeDataString("Consulta médica agendada pelo Health&Med");
            string dataHora = Convert.ToDateTime(notificacao.HorarioConsulta).AddHours(3).ToString("yyyyMMddTHHmmssZ");

            return $"https://www.google.com/calendar/render?action=TEMPLATE&text={titulo}&dates={dataHora}/{dataHora}&details={descricao}";
        }

    }
}
