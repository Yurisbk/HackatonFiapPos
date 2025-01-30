namespace Domain.DTO;

public class DTOHorariosLivre
{
    public int IdMedico;
    public DateTime Horario;

    public override string ToString() => $"IdMedico: {IdMedico} Horario: {Horario}";
}
