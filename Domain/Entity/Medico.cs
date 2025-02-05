namespace Domain.DTO;

public class Medico : Pessoa
{
    public string? CRM;

    public override void Validar()
    {
        base.Validar();

        if (string.IsNullOrWhiteSpace(CRM))
            throw new Exception("CRM inválido");
    }
}
