using Domain.DTO;
using Domain.Entity;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;

namespace Service.Service;

public class ServiceConsulta(IServiceHorarioMedico serviceHorarioMedico, IRepositoryConsulta repositoryConsulta): IServiceConsulta
{
    public class AgendasMedicos
    {
        class HorasLivres
        {
            bool[] Horas = new bool[24];

            public bool this[int hora]
            {
                get => Horas[Math.Clamp(hora, 0, 23)];
                set => Horas[Math.Clamp(hora, 0, 23)] = value;
            }

            public override string ToString() => string.Join(',', Horas.Select((live, hora) => live ? $"{hora}:00" : string.Empty));
        }

        Dictionary<(int idMedico, DateTime data), HorasLivres> Agendas = new();

        HorasLivres GetHorasLivresMedicoNaData(int idMedico, DateTime data)
        {
            if (!Agendas.TryGetValue((idMedico, data.Date), out HorasLivres? horasLivres))
            {
                horasLivres = new();
                Agendas[(idMedico, data)] = horasLivres;
            }

            return horasLivres;
        }

        public bool this[int idMedico, DateTime data]
        {
            get => GetHorasLivresMedicoNaData(idMedico, data.Date)[data.Hour];

            set => GetHorasLivresMedicoNaData(idMedico, data.Date)[data.Hour] = value;
        }

        public DTOHorariosLivre[] ListarHorariosLivres()
        {
            List<DTOHorariosLivre> horariosLivres = new();

            foreach(var agenda in Agendas)
            {
                var idMedico = agenda.Key.idMedico;
                var data = agenda.Key.data;
                var horasLivres = agenda.Value;

                for (int hora = 0; hora < 24; hora++)
                {
                    if (horasLivres[hora])
                        horariosLivres.Add(new DTOHorariosLivre { IdMedico = idMedico, Horario = data.AddHours(hora) });
                }
            }

            return horariosLivres.ToArray();
        }
    }

    public DTOHorariosLivre[] ListarHorariosLivres(int dias = 15)
    {
        AgendasMedicos agendasMedicos = new();

        Dictionary<DayOfWeek, HorarioMedico[]> horariosSemana = new();        

        List<DTOHorariosLivre> horariosLivres = new();

        DateTime dia = DateTime.Now.Date;
        DateTime fim = dia.AddDays(dias);
        while (dia.Date < fim.Date)
        {
            // Armazena horarios medicos da semana
            if (!horariosSemana.ContainsKey(dia.DayOfWeek))
                horariosSemana[dia.DayOfWeek] = serviceHorarioMedico.ListarHorariosMedicoDiaSemana(dia.DayOfWeek);

            var horarios = horariosSemana[dia.DayOfWeek];

            // Registra horarios medicos como "diponivel"
            foreach (var horario in horarios)
            {
                for (int hora = horario.Periodo.HoraInicial.Hours; hora < horario.Periodo.HoraFinal.Hours; hora++)
                    agendasMedicos[horario.IdMedico, dia.Date.AddHours(hora)] = true;
            }

            dia = dia.Date.AddDays(1);
        }

        // Registra consultas marcadas como "ocupado"
        var consultas = repositoryConsulta.ListarProximasConsultas(dias);
        foreach(var consulta in consultas)
            agendasMedicos[consulta.IdMedico, consulta.DataHora] = false;

        return agendasMedicos.ListarHorariosLivres();
    }

    public void RegistrarConsulta(int pacienteId, int medicoId, DateTime horario)
    {
        repositoryConsulta.RegistrarConsulta(new Consulta { IdPaciente = pacienteId, IdMedico = medicoId, DataHora = horario });
    }
}
