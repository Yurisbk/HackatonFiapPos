namespace Domain.DTO
{
    public class DTONotificacao
    {
        public string? EmailMedico { get; set; }
        public string? EmailPaciente { get; set; }
        public string? NomePaciente { get; set; }
        public string? NomeMedico { get; set; }
        public string? HorarioConsulta { get; set; }
        public bool? Confirmacao { get; set; }

        public override string ToString() => $"EmailMedico: {EmailMedico}; EmailPaciente: {EmailPaciente}; NomePaciente: {NomePaciente}; NomeMedico: {NomeMedico}; HorarioConsulta: {HorarioConsulta}; Confirmacao: {Confirmacao}; ";
    }
}
