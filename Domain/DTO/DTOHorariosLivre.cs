namespace Domain.DTO;

public class DTOHorariosLivre
{
    public int IdMedico { get; set; }
    public DateTime Horario { get; set; }
    public double ValorConsulta { get; set; }

    public override string ToString() => $"IdMedico: {IdMedico}; Horario: {Horario}; ValorConsulta:{ValorConsulta}";
}
