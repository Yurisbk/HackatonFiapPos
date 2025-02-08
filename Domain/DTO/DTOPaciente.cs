using Domain.Entity;

namespace Domain.DTO;

public class DTOPaciente
{
    public int? Id { get; set; }
    public string? Nome { get; set; }
    public string? CPF { get; set; }
    public string? EMail { get; set; }

    public static explicit operator Paciente?(DTOPaciente? dtoPaciente)
    {
        if (dtoPaciente == null)
            return null;

        Paciente paciente = new Paciente();
        paciente.Id = dtoPaciente.Id;
        paciente.Nome = dtoPaciente.Nome;
        paciente.CPF = dtoPaciente.CPF;
        paciente.EMail = dtoPaciente.EMail;
        return paciente;
    }

    public static explicit operator DTOPaciente?(Paciente? paciente)
    {
        if (paciente == null)
            return null;

        DTOPaciente dtoPaciente = new DTOPaciente();
        dtoPaciente.Id = paciente.Id;
        dtoPaciente.Nome = paciente.Nome;
        dtoPaciente.CPF = paciente.CPF;
        dtoPaciente.EMail = paciente.EMail;
        return dtoPaciente;
    }
}
