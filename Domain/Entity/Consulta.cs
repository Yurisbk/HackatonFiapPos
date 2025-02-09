using Domain.Enum;

namespace Domain.Entity;

public class Consulta: Entidade
{
    public int IdPaciente { get; set; }
    public int IdMedico { get; set; }
    public DateTime DataHora { get; set; }
    public StatusConsulta StatusConsulta { get; set; }
    public string? JustificativaCancelamento { get; set; }
}
