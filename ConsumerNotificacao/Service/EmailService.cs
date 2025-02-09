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
            string assuntoNegativa = "Health&Med - Consulta cancelada";
            string corpoMedico = GerarConteudoEmail(notificacao, "medico");
            string corpoPaciente = GerarConteudoEmail(notificacao, "paciente");

            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_emailRemetente, _senhaEmail);
                client.EnableSsl = true;

                var mensagemMedico = new MailMessage
                {
                    From = new MailAddress(_emailRemetente),
                    Subject = assunto,
                    Body = corpoMedico,
                    IsBodyHtml = true
                };


                var mensagemPaciente = new MailMessage
                {
                    From = new MailAddress(_emailRemetente),
                    Subject = assunto,
                    Body = corpoPaciente,
                    IsBodyHtml = true
                };


                if (notificacao.Confirmacao is true) 
                {
                    mensagemMedico.To.Add(notificacao.EmailMedico);
                    mensagemPaciente.To.Add(notificacao.EmailPaciente);
                    client.Send(mensagemMedico);
                    client.Send(mensagemPaciente);
                }

                if (notificacao.Confirmacao is false)
                {
                    mensagemPaciente.Subject = assuntoNegativa;
                    mensagemPaciente.To.Add(notificacao.EmailPaciente);
                    client.Send(mensagemPaciente);
                }

            }
        }

        private string GerarConteudoEmail(DTONotificacao notificacao, string pessoa)
        {
            if (notificacao.Confirmacao is true && pessoa == "medico")
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

            if (notificacao.Confirmacao is true && pessoa == "paciente")
            {
                string linkAgenda = GerarLinkAgenda(notificacao);

                return $@"
                <html>
                <body>
                    <h2>Olá, {notificacao.NomePaciente}.</h2>
                    <p>Sua consulta foi marcada com sucesso!</p>
                    <p><strong>Medico:</strong> {notificacao.NomeMedico}</p>
                    <p><strong>Data e horário:</strong> {notificacao.HorarioConsulta}</p>
                    <p><a href='{linkAgenda}' target='_blank'>Clique aqui para adicionar à sua agenda</a></p>
                </body>
                </html>";
            }

            if (notificacao.Confirmacao is false && pessoa == "paciente")
            { 
                return $@"
                <html>
                <body>
                    <h2>Olá, {notificacao.NomePaciente}.</h2>
                    <p>Sua consulta não foi marcada. Sentimos muito.</p>
                    <p><strong>Por favor, escolha outro horario e tente novamente.</p>
                    <p><strong>Obrigado.</p>
                </body>
                </html>";
            }

            return string.Empty;

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
