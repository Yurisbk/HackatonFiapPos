using Domain.Entity;

namespace Domain.DTO;

public class DTOMedico
{
    public int? Id { get; set; }
    public string? Nome { get; set; }
    public string? CPF { get; set; }
    public string? EMail { get; set; }
    public string? CRM { get; set; }
    public string? Especialidade { get; set; }
    public double ValorConsulta { get; set; }

    public static explicit operator Medico?(DTOMedico? dtoMedico)
    {
        if (dtoMedico == null)
            return null;

        Medico medico = new Medico();
        medico.Id = dtoMedico.Id;
        medico.Nome = dtoMedico.Nome;
        medico.CPF = dtoMedico.CPF;
        medico.EMail = dtoMedico.EMail;
        medico.CRM = dtoMedico.CRM;
        medico.Especialidade = dtoMedico.Especialidade;
        medico.ValorConsulta = dtoMedico.ValorConsulta;
        return medico;
    }

    public static explicit operator DTOMedico?(Medico? medico)
    {
        if (medico == null)
            return null;

        DTOMedico dtoMedico = new DTOMedico();
        dtoMedico.Id = medico.Id;
        dtoMedico.Nome = medico.Nome;
        dtoMedico.CPF = medico.CPF;
        dtoMedico.EMail = medico.EMail;
        dtoMedico.CRM = medico.CRM;
        dtoMedico.Especialidade = medico.Especialidade;
        dtoMedico.ValorConsulta = medico.ValorConsulta;
        return dtoMedico;
    }
}
