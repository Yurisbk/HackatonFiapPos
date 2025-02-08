using Domain.DTO;
using Domain.Entity;
using Domain.Enum;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Service.Helper;

namespace Service.Service;

public class ServiceConsulta(
    IRepositoryConsulta repositoryConsulta,
    IServiceHorarioMedico serviceHorarioMedico,
    IServiceCadastroMedico serviceCadastroMedico,
    HelperTransacao helperTransacao) : IServiceConsulta
{
    public async Task<DTOHorariosLivre[]> ListarAgendaMedico(int idMedico, int dias = 7)
    {
        DateTime data = DateTime.Today;

        List<DTOHorariosLivre> horariosLivre = new();

        if (dias < 1)
            throw new ArgumentException("numero de dias deve ser maior que 0.");

        var medico = await serviceCadastroMedico.ResgatarMedicoPorId(idMedico);
        if (medico == null)
            throw new ArgumentException("Médico não encontrado.");

        Dictionary<DayOfWeek, HorarioMedico[]> bufferHorariosDiaSemana = new();

        while ((data.Date - DateTime.Today).Days < dias)
        {
            List<int> horariosLivres = new();

            // Verifica se já tem os horarios do medico no dia da semana em buffer, evitando carregamento duplicado
            if (!bufferHorariosDiaSemana.TryGetValue(data.DayOfWeek, out HorarioMedico[] horariosMedico))
            {
                horariosMedico = await serviceHorarioMedico.ResgatarHorariosMedicoDiaSemana(idMedico, data.DayOfWeek);
                bufferHorariosDiaSemana.Add(data.DayOfWeek, horariosMedico);
            }

            // Cria um array com todas as horas livres do médico no dia
            int[] horasLivre = horariosMedico.SelectMany(horario => Enumerable.Range(horario.Periodo.HoraInicial.Hours, horario.Periodo.HoraFinal.Hours)).ToArray();

            // Lista as consultas do medico no dia e remove as horas ocupadas
            var consultas = await repositoryConsulta.ListarConsultasAtivasMedico(idMedico, data.Date);
            horasLivre = horasLivre.Except(consultas.Select(consulta => consulta.DataHora.Hour)).ToArray();

            // Adiciona os horarios livres na lista de retorno
            horariosLivre.AddRange(horasLivre.Select(h => new DTOHorariosLivre() { Horario = new DateTime(data.Year, data.Month, data.Day, h, 0, 0), IdMedico = idMedico }));

            data = data.AddDays(1);
        }

        return horariosLivre.ToArray();
    }

    public async Task GravarStatusConsulta(int idConsulta, StatusConsulta statusConsulta, string justificativaCancelamento)
    {
        using (var transacao = helperTransacao.CriaTransacao())
        {
            if (statusConsulta == StatusConsulta.Pendente)
                throw new ArgumentException("Status de consulta inválido.");

            Consulta? consulta = await repositoryConsulta.ResgatarConsultaPorId(idConsulta);
            if (consulta == null)
                throw new ArgumentException("Consulta não encontrada.");

            if ((statusConsulta == StatusConsulta.Agendada || statusConsulta == StatusConsulta.Recusada) && consulta.StatusConsulta != StatusConsulta.Pendente)
                throw new InvalidOperationException("Não é possivel aceitar ou recusar consulta que não esteja pendente.");

            if (statusConsulta == StatusConsulta.Cancelada && consulta.StatusConsulta != StatusConsulta.Agendada)
                throw new InvalidOperationException("Não é possivel cancelar consulta não agendada.");

            consulta.StatusConsulta = statusConsulta;
            consulta.JustificativaCancelamento = justificativaCancelamento;

            await repositoryConsulta.GravarStatusConsulta(consulta);

            transacao.Gravar();
        }
    }

    public async Task<Consulta[]> ListarConsultasPendentesConfirmacaoMedico(int idMedico)
        => await repositoryConsulta.ListarConsultasPendentesConfirmacaoMedico(idMedico);

    public async Task<Consulta[]> ListarConsultasAtivasPaciente(int idPaciente)
        => await repositoryConsulta.ListarConsultasAtivasPaciente(idPaciente);

    public async Task<Consulta[]> ListarConsultasAtivasMedico(int idMedico, DateTime? data)
        => await repositoryConsulta.ListarConsultasAtivasMedico(idMedico, data);
}
