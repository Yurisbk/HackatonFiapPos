﻿using Domain.DTO;
using Domain.Entity;
using Domain.Enum;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Service.Helper;

namespace Service.Service;

public class ServiceConsulta(
    IRepositoryConsulta repositoryConsulta,
    IServiceHorarioMedico serviceHorarioMedico,
    IRepositoryMedico repositoryMedico,
    HelperTransacao helperTransacao) : IServiceConsulta
{
    public async Task<DTOHorariosLivre[]> ListarAgendaMedico(int idMedico, int dias = 7)
    {
        DateTime data = DateTime.Today;

        List<DTOHorariosLivre> horariosLivre = new();

        if (dias < 1)
            throw new ArgumentException("numero de dias deve ser maior que 0.");

        var medico = await repositoryMedico.ResgatarMedicoPorId(idMedico);
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

    public async Task RegistrarConsulta(int idMedico, int idPaciente, DateTime data)
    {
        using (var transacao = helperTransacao.CriaTransacao())
        {
            // Arredonda data para horario cheia
            data = new DateTime(data.Year, data.Month, data.Day, data.Hour, 0, 0);

            var horarios = await serviceHorarioMedico.ResgatarHorariosMedicoDiaSemana(idMedico, data.DayOfWeek);

            // Verifica se medico atende no dia da semana
            if (horarios == null || horarios.Length == 0)
                throw new InvalidOperationException("Médico não atende neste dia da semana.");

            // Verifica se médico atende no horario solicitado
            if (!horarios.Any(horario => horario.Periodo.ContemHora(data.Hour)))
                throw new InvalidOperationException("Médico não atende no horário solicitado");

            // Verifica se médico já tem consulta marcada no horário solicitado
            var consultas = await repositoryConsulta.ListarConsultasAtivasMedico(idMedico, data.Date);
            if (consultas != null && consultas.Length > 0)
            {
                if (consultas.Any(consulta => consulta.DataHora.Hour == data.Hour))
                    throw new InvalidOperationException("Médico já tem consulta marcada neste horário.");
            }

            // Verifica se paciente ja tem consulta marcada no horario
            consultas = await repositoryConsulta.ListarConsultasAtivasPaciente(idPaciente, data.Date);
            if (consultas != null && consultas.Length > 0)
            {
                if (consultas.Any(consulta => consulta.DataHora.Hour == data.Hour))
                    throw new InvalidOperationException("Paciente já tem consulta marcada neste horário.");
            }

            Consulta consulta = new Consulta()
            {
                DataHora = data,
                IdMedico = idMedico,
                IdPaciente = idPaciente,
                StatusConsulta = StatusConsulta.Pendente
            };

            await repositoryConsulta.RegistrarConsulta(consulta);

            transacao.Gravar();
        }
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

    public async Task<Consulta[]> ListarConsultasAtivasPaciente(int idPaciente, DateTime? data = null)
        => await repositoryConsulta.ListarConsultasAtivasPaciente(idPaciente, data);

    public async Task<Consulta[]> ListarConsultasAtivasMedico(int idMedico, DateTime? data = null)
        => await repositoryConsulta.ListarConsultasAtivasMedico(idMedico, data);
}
