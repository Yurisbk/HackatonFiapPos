namespace Domain.DTO;

public class DTOHorariosLivre
{
    public int IdMedico;
    public DateTime Horario;
    public double ValorConsulta;

    public override string ToString() => $"IdMedico: {IdMedico}; Horario: {Horario}; ValorConsulta:{ValorConsulta}";
}
